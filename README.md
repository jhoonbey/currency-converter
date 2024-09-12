# Currency Converter

There are 2 applications: 
- **CurrencyConverter**
- **CurrencyConverter.Tests**

`CurrencyConverter` is the main application that contains 3 endpoints to get data from Frankfurter API. 
`CurrencyConverter.Tests` contains tests.

## Tech

- [C#]
- [NET 8]
- [Redis]
- [Refit]
- [LINQ]
- [Swagger]
- [Polly]
- [xUnit]

## Run

Just run the docker-compose file to run the environment.
```sh
  docker-compose up --build
```  

_If you want to run the application locally without a docker-compose file, make sure you have a Redis running in the local machine_

## Swagger:

After running the application, go to use:
http://localhost:5000/swagger/index.html