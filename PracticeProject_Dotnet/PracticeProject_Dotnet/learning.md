LEARNING
    #REST PRINCIPLES




# REST PRINCIPLES

1. Rest is a architectural concept and it is not a framework, just follow to develop API so that it will be easier to build api.
    1. Advantages
        1. Simple       - Uses http verbs (get, post, put, delete)
        2. Scalable     - Stateless design makes horizontal scaling
        3. Flexible     - Supports multiple formats (JSON, XML,..)
        4. Cacheable    - we can use http built-in cache
        5. Decouple     - FE and BE are completely independent.

2. Rest have six constaints to follow
    
    1. Client/Server Independent
        1. Client doesnt need to know what server will do to store data or perfrom some tasks and server doesnt need to know what client UI is made of.
        2. If we wrote api based on UI framework for example, creating api that supports only for web application and if we are trying to use the api for mobile UI and that would not support resulting additional work

    2. Statelessness
        1. Storing records in session is not a good approach, so depending on client data is not ok.
        2. To avoid that, send all kind of relevant infos while triggering the request, so that server can use the data to handle the request. For example instead of storing user role in session, we can send the user role in jwt token so that we can understand the role of the user.
        3. To summarize, the client must send all kind of data to server so that, the server should not have the need to fetch from session and it can handle request based on the input data we receive

    3. Cacheability
        1. While sending response, the response should contain the information whether it was cached or not, so that instead of sending multiple request to the same api, we can avoid in client side.

    4. Uniform interface
        1. Most important constraint in REST

            1. Resource identification
                1. make sure the uri are understandable and explainable
            
            2. Resource Manipulation
                1. Share want client needs, for example, we have multiple fields in the user object, name, password etc.. So instead of sharing everything, make sure you are sharing the necessary stuffs

            3. Self-descriptive messages
                1. Include all the necessary infos about the response in the header, for example the response type whether it was a json or not, whether it is a cached response with the timelimit so on

            4. HATEOAS
                1. least implemented one. lets say we are creating some user and the concept of hateoas is to share the get link for the user post creating; Meaning - send relevant links to the response if required

    5. Layered System
        1. Client doesnt need to know whether it was directly connecting to server or reverse proxy servers or load balancers, it should send the request with all the sufficient data. 

    6. Code on demand
        1. Send js or any kind of code to client itself. For example. In Myte, used to send the drop down types and content for the page

3. Best practices to develop REST API
    1. Use headers
        1. GET      - to retrive a unmodified response
        2. PUT      - to replace the exising resource, but at the end modifies the          same resource how many times we trigger
        3. POST     - to create a completely new resource, everytime it triggers, creates a new record
        4. PATCH    - to modify some of the content in the resource
        5. DELETE   - to remove the entire resource 

    2. Use singluar noun for singular activity for example, instead of mentioning as getUser make is as users, since our method type explains what kind of action we are going to do
    
    3. Same as above, use plural nouns for collections
    
    4. Use path parameters to fetch the exact data and use query parameters for sorting/filtering/paging
        1. Path parameters  - api/user/12
        2. Query parameters - api/user?age=12&gender=male

4. API Versioning
    1. make versions so that avoid chaos while developing new set of api
    2. Implement versioning by two approaches
        1. URL path version     - example: api/v1/users
            1. use when we need to implement cache, explicit
        2. Query versioning     - example: api/users?v=0.1
            1. use when we need simpler approach
        3. Header versioning    - example: {
            x-API-version: 1.0
        }   
            1. use when we need cleaner approach, but requires documentation


# Configuration 

1. It contains layered configuration system, each with higher precedence override the previous one
    1. appsettings.json is the first one to get loaded and it will be overridden by appsetting.development.json/ appsettings.production.json based on the env

    2. This appsettings.development/production.json will get overridden by environment variables

    3. Enivornment variables are overridden by command line arguments.

2. var builder = WebApplication.CreateBuilder(args); is responsible for loading the configuration

3. This variable "ASPNETCORE_ENVIRONMENT" contains the information about the environment which we are working on. Based on this, the appsettings.{ENV}.json will get loaded. Initially appsettings.json will get loaded then env is fetch from above variable and that file get loaded and override the appsettings.json file

4. to launch the profile
    1. dotnet run --launch-profile "https"
    2. dotnet run --launch-profile "Production-Local"

5. Use options pattern
    1. Sometimes, if anyone changes the key in the appsettings.json that will cause an error or if anyone tries to access the key by mismatching with cases, that will cause an error, so try to use options pattern to access the key.


# Middlewares

1. Middlewares process the request and proceeds the request further.

2. The order is mandatory so that everything run smoothly.

3. The two main things required for exception is that
    1. RequestDelegate - Moves the request to next middleware
    2. HttpContext - contains all the details about the request and response

3. we can implement custom middleware as well. We have multiple approach
    1. Convention based:
        1. Create a standard class where it accepts RequestDelegate as in primary constructor and create an async method which accepts
            1. public class ConventionMiddleware(RequestDelegate request){
                public async Task InvokeAsync(HttpContext context){
                    //logic
                    await request(context);
                }
            }

            2. in program.cs
                app.UseMiddleware<ConventionMiddleware>
    
    2. Extension middlware
        1. Create a static class and a static extension method refers to IApplicationBuilder and returns the IApplicationBuilder, so that we can use the method UseExtensionMiddleware directly in program.cs
            1. public static class Extension {
                public static IApplicationBuilder UseExtensionMiddleware(this IApplicationBuilder app){
                    app.UseMiddleware<ConventionMiddleware>
                }
            }

            2. in program.cs
                app.UseExtensionMiddleware();

4. Note: The above two approach is ok, but the lifetime scope is set to singleton by default, so that once instance created, it will be for all the requests. Since the instance is singletion, we can't inject any scoped services. so we have an alternate approach
    1. IMiddleware Interface
        1. Since we can set lifetime scope of this approach, we need to mention the same in the services and in the middleware
            1. public class CorrelationBasedMiddleware : IMiddleware
            {
                public async Task InvokeAsync(HttpContext context, RequestDelegate next)
                {
                    //Console.WriteLine("This is from correlation middleware -- Started");
                    await next(context);
                    //Console.WriteLine("This is from correlation middleware -- Ended");
                }
            }
        2. under services
             builder.Services.AddTransient<CorrelationBasedMiddleware>();

        
        3. under middleware
            app.UseMiddleware<CorrelationBasedMiddleware>();

5. We have middleware, action filters and endpoint filters to process the incoming request, use based on requirement
    1. Middleware 
        1. Applies for all http pipeline
    2. Action Filters
        1. For MVC controllers only
    3. Endpoint filters
        1. Per minimal api end point

6. Most important, please follow the below order
    var app = builder.Build();

    app.UseExceptionHandler();                   // 1. Catch all unhandled exceptions
    app.UseHttpsRedirection();                   // 2. Redirect HTTP → HTTPS
    app.UseRouting();                            // 3. Match routes
    app.UseCors();                               // 4. CORS headers
    app.UseAuthentication();                     // 5. Establish identity
    app.UseAuthorization();                      // 6. Check permissions
    app.UseOutputCache();                        // 7. Serve cached responses
    app.UseRateLimiter();                        // 8. Enforce rate limits
    app.UseResponseCompression();                // 9. Compress responses
    app.UseMiddleware<RequestLoggingMiddleware>();// 10. Custom middleware
    app.MapControllers();                        // 11. Execute endpoints

    app.Run();

7. Always use built-in middlewares

# Dependency Injection

1. it is a design pattern to help us to manage, test and scale easily. Instead of classes creating instance, DI creates instances and shares with classes.

2. to summarize, DI creates, manages and shares the instance with the classes which they need

3.  this makes loose couple, for example, lets say we are sending an email with the class EmailSender, now we changes to EmailSenderEnhanced, if we create this instance in every class that makes tight coupling and tough to test, if we use DI concept, for example, create an interface and inherites from the interface and assign the lifetime scope in services, to changes we just need to change one line.
    1. without DI:
        1. before changing
            public class NotificationService
            {
                private readonly EmailSender _emailService = new();

                public void Send() => _emailService.SendEmail();
            }
        2. after changing
            public class NotificationService
            {
                private readonly EmailSenderEnhanced _emailService = new();

                public void Send() => _emailService.SendEmail();
            }
    
    2. With DI:
        1. Before changing  -- builder.services.AddTransient<IEmailSender, EmailSender>();
        2. After changing   -- builder.services.AddTransient<IEmailSender, EmailSenderEnhanced>();
        3. In class for before/after changing
            public class NotificationService(IEmailSender emailService)
            {
                public void Send() => emailService.SendEmail();
            }
        
4. We have 3 different types:
    1. constructor injection
        1. recommended one, injected the instance via constructor
    2. method injection
        1. this is for minimal api
    3. property injection

5. We can inject DI in multiple ways
    1. directy under program.cs
        builder.services.AddScoped<SomeClass>
    2. Using extension approach
        1. public static class ExtensionService{
            public static IServiceCollection CustomizedService(this IServiceCollection service){
                service.AddScoped<SomeClass>
            }
        }

6. There are some services which were injected automatically by .net core, those were as follows
    1. These are called framework services
        1. ILogger<T> for structured logging
        2. IConfiguration for accessing app settings
        3. IWebHostEnvironment for environment-specific behavior
        4. IHttpContextAccessor for accessing the current HTTP context
        5. IOptions<T> for binding configuration to typed objects

    2. These are called application services
        1. our normal classes and interfaces

7. In DI, use interface driven approach, instead of this, do the following
    
    1. without interface driven
        1. In class
            public class SampleClass(EmailSender sender){}
        2. In program.cs
            builder.services.AddScoped<EmailSender>();
        
    2. with interface driven
        1. In class
            public class SampleClass(IEmailSender sender){}
            public class EmailSender : IEmailSender{}
        
        2. In program.cs
            builder.services.AddScoped<IEmailSender, EmailSender>

    3. interface driven DI makes loose coupling

# Service lifetime

1. We have three types of service lifetime

    1. Transient
        1. Each time, whenever DI request came, it will create a new instance. for example, if class A asks for Instance for Class 1, it will create new instance, if class B asks for Instance for Class 1, it will create a fresh new copy, the highlight is this will happen within a request. lets say a api request came and we asked Class 1 instance multiple time from multiple classes (A, B, ...), so it will create multiple instances within a request itself. Useful for EmailBuilder

    2. Scoped
        1. A single instance is created for a entire request, no matter how many times Class 1 called in Class A, Class B, only one instance for the entire request, if new request came, new instance for Class 1 will get trigger. Useful for DBContext, caching

    3. Singleton
        1. A single instance created for an entire app, no matter how many calls trigger within request or multiple requests trigger, it will create only one instance and if application restarts, it will create a new instance. Useful for logging, IConfigurationService

# Keyed Services

1. it is a built in feature in DI, so basically lets say we have a interface and that was inherited by multiple classes, so to fetch the correct one, we can use both apporach.
    
    1. factory approach -- creates the boilerplate and selects the class based on the user input. mostly object creation and object selection happens in business logic.. we can discuss later

    2. keyed service - lets say we have multiple classess inherited to our interface and we want to select a single instance, it can be based on user input, but if we dont want to pass anything to those classes in constructor and everything should be maintained by DI, we can go with keyed service. 


2. The example is added.

# Filters

1. Middlewares are rules which will execute for all incoming request, it did check whether it was a proper request or not, but it will do for all the incoming request, it doesnt skip for particular request. once middleware validates, it redirects to the controller for processing the request, our filters interrupt and performs some custom logic on the request, it have multiple filter types and each have some tasks to do. This is the purpose of the filters.

2. we have six type of filters, and each run on the below mentioned flow
    1. Authorization Filters - run first, before everything else
    2. Resource Filters - run before model binding, can short-circuit the entire pipeline
    3. Action Filters - run immediately before and after an action method executes
    4. Exception Filters - catch unhandled exceptions thrown during action execution
    5. Result Filters - run before and after the action result is executed
    6. Endpoint Filters - the lightweight alternative for Minimal APIs, introduced in .NET 7

3. Detailed Explanation below
    1. Authorization runs once routing middleware happens, its for additional check to allow or not to allow the request.
    2. Resource filter will run when the model binding happens, when request came, it still be json, the model binding will covert the json into objects, so if we need any kind of validation on model binding we can go with resource filter
    3. Action filter will run before triggering our api, our auth was successfull, our binding was successfull and we are going to proceed our logic, before that we can use action filter to do some tasks like validating the input
    4. Exception filter will trigger whenever the there is a exception in the logic, it is advised to use global exception handling
    5. Result Filter is to alter the result post api gives the response, our business logic ran and we need to add some additional details without changing any structure, so what can we do is to trigger a result filter
    6. Endpoint filter is for minimal api filter
        Request arrives
            │
            ▼
        1. Authorization filter   ← before ANYTHING else
            │
            ▼
        2. Resource filter        ← before model binding (body not read yet)
            │
            ▼
        [model binding happens — body is deserialized here]
            │
            ▼
        3. Action filter          ← after model binding, before action
            │
            ▼
        [action method runs]
            │
            ▼
        4. Exception filter       ← only if something threw
            │
            ▼
        5. Result filter          ← before/after response is written

4. Overall order
    --> Authorization Filters
        --> Resource Filters (OnResourceExecuting)
            --> Model Binding
                --> Action Filters (OnActionExecuting)
                    --> Action Method
                <-- Action Filters (OnActionExecuted)
            --> Result Filters (OnResultExecuting)
                --> Result Execution
            <-- Result Filters (OnResultExecuted)
        <-- Resource Filters (OnResourceExecuted)

5. As we see in the code, inorder to apply the filter, we can go with two approach
    1. global approach
        1. it will run for all the controller and all the api, mimic as same as middleware but can able to access controller actions
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<TimingResourceFilter>();
                options.Filters.Add<GlobalExceptionFilter>();
                options.Filters.Add<ResponseWrappingResultFilter>();
            });

    2. approach specific to controller/action methods
        1. To apply, what we need to do is inherit our method with any one of the filter for example
            public class ResourceFilter : IAsyncResourceFilter
        2. Post that, to involve this filter, we need to add it in the top of the controller/action methods, we have two approaches
            
            1. ServiceFilter
                1. [ServiceFilter(typeof(ResourceFilter))]
                2. In this way we can invoke it, but the thing is we need to register our service and we cant pass any arguments to the filter classess
                    builder.Services.AddScoped<LoggingActionFilter>();
                3. If we are not registering, it will throw run time error
            
            2. TypeFilter
                1. We dont need to perform DI injection
                2. we can pass arguments as well
                    [TypeFilter(typeof(ResourceFilter), Arguments = new object[] { "Admin" })]


# Standardizing API Error Response

1. Instead of maintaning multiple error formats for multiple api's and that would cause inconsistency and makes huge mess, we can have a standardizing approach to send error messages to client, below is the standard RFC 7807 format
    1. {
        "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
        "title": "Not Found",
        "status": 404,
        "detail": "The user with ID 123 was not found.",
        "instance": "/api/users/123"
        }
    2. Explanation to the above properties
        type        → A URI that identifies the problem type. (Could point to documentation, or to the RFC section itself.)
        title       → A short, human-readable summary of the problem.
        status      → The HTTP status code (400, 404, 500, etc.).
        detail      → A human-readable explanation of what went wrong.
        instance    → The specific request path or resource that caused the error.

3. To implement this functionality, we need to add in the program.cs to invoke it.
    builder.Services.AddProblemDetails()

# Exception Handling

1. We have multiple ways to perform exception handling, using try/cactch in every method. or creating extension exception class and we can inject in the middleware are using IExceptionHander.

2. Based on the version of .net we can decide, if the application version is 8+ we can go with IExceptionHandler if it is lower, we can go with custom exception handler or creating a class by inheriting with IMiddleware
    
    1. If we are going with custom middleware, we need to write the method which should be equals to either "invoke" or "invokeasync", changing this will compile fine but middleware got ignored.
        app.UseMiddleware<CustomExtensionExceptionHandler>();
        
    2. If we really want to change the method name present in our class, inherit the IMiddleware class, but the thing is we need to inject the service with lifetime scope and we need to inject in our middleware
        // Program.cs
        builder.Services.AddTransient<CustomExtensionExceptionHandler>();
        app.UseMiddleware<CustomExtensionExceptionHandler>();

    3. if our .net is more than 8+ go with IExceptionHandlder

        1. create a class and inherit with IExceptionHandler, write the necessary logic and inject the service and add in middleware
            1. public class OurClass(IProblemDetailsService service) : IExceptionHandlder{
                //implement the interface and return the method with true but add our exception details in the ProblemDetails class.
            }
            2. builder.Services.AddExceptionHandler<OurClass>();
            3. app.UseExceptionHandler<OurClass>();

# Structured logging

1. Use serilog for structured login. Install serilog with the below command
    1. dotnet add package Serilog.AspNetCore --version 10.0.0

2. This single package contains all the necessary contents

3. Inorder to implement the serilog, we need to implement two stage bootstrap patter:

    1. While application got started, it doesnt load the appsettings.json so it doesnt know what are mentioned inorder to log, so to log, we need to log temporarily for that we need to use the code below
        
        1.  Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
        
        2. Here WriteTo.Console() is responsible for logging in console, inorder to logging into file, we need to do the following
            1. .WriteTo.File(path:"logs/logs.txt", rollingInterval: RollingInterval.Minute, retainedFileCountLimit: 7)

    2. Injecting into service
        
        1. Here, we add the serilog into the service and gave two kind of access, 
             ReadFrom.Configuration(builder.Configuration) -> this is to access the configuration which we mentioned in the appsettings.json

            ReadFrom.Services(service) -> this is to provide access the DI service related to Enricher, the reason is since serilog is 3rd party one, it doesnt know how DI works for enricher. Enricher is nothing but a log kind of configuration that will present for all the logs which we are going to insert into the files.

    3. Injecting into middleware

         app.UseSerilogRequestLogging();        

        
        builder.Services.AddSerilog((service, loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(builder.Configuration).
                ReadFrom.Services(service);
            });

    
4. Prompt to add enrichers
    dotnet add package Serilog.Enrichers.Environment --version 3.0.1
    dotnet add package Serilog.Enrichers.Thread --version 4.0.0
    dotnet add package Serilog.Enrichers.Process --version 3.0.0
    dotnet add package Serilog.Exceptions --version 8.4.0

    so we have added these enrichers, so going forward, whatever logs, it included these variables as well, 

5. Always try to implement the structured logging instead of plain text logs

