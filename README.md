# Pruebas de componentes Blazor (bUnit)
Al igual que el resto del código, los componentes Blazor son una parte a tener en cuenta a la hora de planificar una correcta estrategia de testing en los proyectos.

Dos enfoques habituales para probar dichos componentes son:
 - **Pruebas unitarias.** Estas se escriben con una biblioteca de pruebas unitarias, que en nuestro caso será [bUnit](https://bunit.dev/index.html).
 - **Pruebas E2E.** Un ejecutor de pruebas ejecuta una aplicación Blazor que contiene el CUT y automatiza una instancia del explorador. La herramienta de pruebas inspecciona el CUT e interactúa con él a través del explorador. [Playwright for .NET](https://playwright.dev/dotnet/) es un ejemplo de un marco de pruebas de E2E que se puede usar con aplicaciones Blazor

## Pero, ¿Qué es bUnit?

bUnit es una librería gratis para el testing de componentes Blazor. Su objetivo es permitir redactar de manera fácil y comprensible los test unitarios para estos componentes.

Entre sus ventajas encontramos:
 - Definir pruebas bajo sintaxis C# y/o Razor.
 - Verificar outputs usando semántica HTML.
 - Interactuar e inspeccionar componentes, triggers y events.
 - Pasar parámetros a componentes y obtener trazabilidad de los mismos.
 - Mocks de componentes, servicios, IJSRuntime y hasta de Blazor authentication and authorization.

Las pruebas de bUnit se pueden ejecutar desde:

 - Explorador de pruebas de Visual Studio.
 - El comando [`dotnet test`](https://learn.microsoft.com/es-es/dotnet/core/tools/dotnet-test)
 - Canalización DevOps automática.

> **Nota** *bUnit es una biblioteca de pruebas de terceros y no es compatible
> con Microsoft, que tampoco la mantiene*.

## Me interesa bUnit, ¿Cómo empiezo?

Para empezar a escribir test con bUnit, se necesita lo siguiente:

 - Crear un nuevo proyecto bUnit.
 - Escribir los test para los componentes Blazor.

### Crear un nuevo proyecto.

> En esta guía se muestran los pasos para crear un proyecto con Visual
> Studio, no obstante; en la documentación oficial viene detallada
> también la forma manual de creación.

Los pasos necesarios son:

1. **Instalar la plantilla**. Este paso se puede realizar desde NuGet
``
dotnet new --install bunit.template
``
2. **Crear un nuevo proyecto**. Una vez tengamos la plantilla, en Visual Studio aparecerá la opción de nuevo proyecto bUnit. Siguiendo los pasos del Wizard podremos elegir los siguientes Frameworks:
 - [xUnit](https://xunit.net/)
 - [NUnit](https://nunit.org/)
 - [MSTest](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest)
4. **Añadir el proyecto a tu solución**. En caso de que no lo hayamos creado ya bajo una solución existente.

### Escribir los test, ¿C# o Razor?
Probar los componentes de Blazor es un poco diferente de probar las clases de C#: los componentes de Blazor se renderizan y tienen el ciclo de vida del componente Blazor durante el cual podemos proporcionarles entrada y producir salida.

Esta renderización ocurre a través del **TestContext** de bUnit. El resultado de la renderización es un **IRenderedComponent**, referido como un “componente renderizado”, que proporciona acceso a la instancia del componente y al marcado producido por el componente.

bUnit funciona con MSTest, NUnit y xUnit, por lo que permite escribir las pruebas unitarias en archivos .cs o .razor.

Escribir pruebas en archivos .razor, proporciona una forma más fácil de declarar el marcado de componentes y HTML en las pruebas, por lo que probablemente será la opción preferida para muchas personas en el futuro.

> **Nota** Sin embargo, el editor Razor actual en Visual Studio 2022 no ofrece todas las características de edición de código disponibles en
> el editor de C#, por lo que es algo a considerar.

Para explicar mejor esta casuística, podremos ver en el proyecto las diferencias entre escoger un sistema u otro accediendo a las clases alojadas bajo Pages > GettingStarted.

## Parámetros
bUnit permite una gran variedad de formas validar el paso de parámetros entre componentes.

En el proyecto, accediendo a las clases alojadas bajo Pages > ProvidingInput, aparecerán los ejemplo del testeo de componentes que requieren parámetros para su funcionamiento.
 
## Servicios
Es común que los componentes tengan una o varias dependencias servicios.

    @inject IMyService MyService
    [Inject] private IMyService MyService { get; set; }

Con bUnit a través de la colección de Servicios, gracias al TestContext; podremos inyectarlos. 
La colección de Servicios es solo un **IServiceCollection**, lo que significa que los servicios pueden registrarse de la misma manera que se hace para el código de producción en Startup.cs en proyectos de Blazor Server y en Program.cs en proyectos de Blazor WASM.

> **Nota 1** En bUnit, registras los servicios en la colección de Servicios antes de renderizar un componente bajo prueba.
> 
> **Nota 2** El método AddSingleton() solo está disponible en la colección de Servicios si se importa el espacio de nombres
> **Microsoft.Extensions.DependencyInjection** en la clase de prueba.

Ejemplos de esto los encontraremos en el proyecto > Pages > ProvidingInput > InjectServicesTest.cs

## Mockeando componentes. STUBS, ¿Qué son?
Al igual que se hace con el resto de código, es posible mockear elementos que no se han de probar necesariamente en las pruebas unitarias de componentes.

Para realizar esto, si bien podemos usar la librería Moq como siempre, bUnit implementa los STUBS.

Los  `Stub<>`  en bUnit son una funcionalidad incorporada que permite sustituir componentes durante las pruebas de manera similar a los Mocks.

En Pages > ProvidingInputs > SubstitutingTests.cs estarán los ejemplos para el uso de ambos componentes.

## Triggers, Events & Renders
Blazor hace posible vincular manejadores de eventos a componentes

    @onclick=“MyClickHandler”.

bUnit tiene métodos auxiliares que hacen posible invocar todos los tipos de eventos soportados por Blazor.

Para usarlos, primero se ha de encontrar donde está vinculado el evento para, a continuación; invocarlo.

En Pages > Interaction > TriggeringCustomEventsTest.cs aparecerán los ejempo de invocación de eventos.

Por otra parte, también es posible renderizar el componente N veces según necesitemos para poder aseverar un correcto funcionamiento del mismo con las pruebas unitarias.

Esto se consigue con los métodos Render(), SetParametersAndRender(), o usando inderectamente InvokeAsync().

Estos ejemplos están recogidos en Pages > Interaction > TriggeringRendersTest

## Waiting State

Un test puede fallar si un componente realiza renderizados asíncronos. Esto puede deberse, por ejemplo, si un componente está esperando que un servicio web asíncrono le devuelva datos en el OnInitializedAsync() antes de renderizarlo.

Para ello es necesario manejar esto específicamente en las pruebas, puesto que estas, se ejecutan en el contexto de sincronización del marco de pruebas y el renderizador ejecuta los renders en su propio contexto. Si no lo haces, es probable que experimentes pruebas con éxito aleatorio.

bUnit tiene dos métodos que ayudan a lidiar con este problema: WaitForState()  WaitForAssertion().

Algunos ejemplos de esto se pueden encontrar en Pages > Interation > AsynchronousStateChangeTest
