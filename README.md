# MediatR.Strategies

MediatR.Strategies is a CSharp extension for Mediator that enables parallel, asynchronous and synchronous

## Installation

Use the package manager [nuget](https://www.nuget.org/) to install MediatR.Strategies.

```sh
dotnet add package MediatR.Strategies
```

## Usage
First, we need to register the services. AddPublisher() takes as input a function that configures MediatR.
```csharp
IServiceCollection services = new ServiceCollection(); 
services.AddPublisher((config) => { });

IServiceProvider provider = services.BuildServiceProvider();

IPublisher publisher = provider.GetRequiredService<IPublisher>();

await publisher.Publish(new CustomNotification(), PublishStrategy.Async);
``` 
Next, we get an instance of the Publisher class and call the Publish() method in it, this method takes two parameters, the first is an instance of the notification class to be distributed, the second is the notification distribution strategy
## Contributing

Pull requests are welcome. For major changes, please open an issue first
to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License

[MIT](https://choosealicense.com/licenses/mit/)
