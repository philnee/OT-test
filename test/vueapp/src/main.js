import { createApp } from 'vue'
import App from './App.vue'

const { WebTracerProvider } = require('@opentelemetry/sdk-trace-web');
const { getWebAutoInstrumentations } = require('@opentelemetry/auto-instrumentations-web');
const { ZipkinExporter } = require('@opentelemetry/exporter-zipkin');
const { SimpleSpanProcessor } = require('@opentelemetry/sdk-trace-base');
const { registerInstrumentations } = require('@opentelemetry/instrumentation');
const { ZoneContextManager } = require('@opentelemetry/context-zone');
const { Resource } = require("@opentelemetry/resources");
const { SemanticResourceAttributes } = require("@opentelemetry/semantic-conventions");

const options = {
    headers: {
        'my-header': 'header-value',
    },
    // optional interceptor
    getExportRequestHeaders: () => {
        return {
            'my-header': 'header-value',
        }
    },
}
const exporter = new ZipkinExporter(options);
const resource =
    Resource.default().merge(
        new Resource({
            [SemanticResourceAttributes.SERVICE_NAME]: "Vue web app",
        })
    );

const provider = new WebTracerProvider({
    resource: resource,
});
provider.addSpanProcessor(new SimpleSpanProcessor(exporter));
provider.register({
    contextManager: new ZoneContextManager(),
});

registerInstrumentations({
    instrumentations: [
        getWebAutoInstrumentations({
            // load custom configuration for xml-http-request instrumentation
            '@opentelemetry/instrumentation-xml-http-request': {
                clearTimingResources: true,
            },
        }),
    ],
});

createApp(App).mount('#app')
