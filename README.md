# TqShoppingBasket API

## _Domain-Driven Design Architecture Demo_

### Tq.ShoppingBasket.API
- user interface and entry point from user point of view
- depends on Application and Infrastructure layer
- hosts Controllers, logging setup and Middleware depending on needs
- if needed another project can be added such as MVC
### Tq.ShoppingBasket.Application
- application layer hosts Queries and Handlers for CQRS pattern, various View Models, Validators, Mappers and Services consumed in presentation layer (in this case it's API)
### Tq.ShoppingBasket.Domain
- hosts Domain entity models, Exception and  Repository Abstractions
- has no external dependencies
### Tq.ShoppingBasket.Infrastructure
- used for ORM and data access
- in this test solution we have a simple repository implementation that mimics DB access
- we can also implement any other external service like Payment, Email, File Storage etc.
- can be expanded to additional Persistence layers for specific DB access SQL, Postgres etc.
### Tq.ShoppingBasket.Tests
- tests for single service

# _Notes about implementation_

- API endpoint added
- initial Class library refactored to follow _S.O.L.I.D_ principles
- test names refactored to give better idea on what's tested with _AAA_ pattern
- strategy design pattern used for _ShoppingBasketService_
- serilog setup for console and file logging
- .Net 5
- runs in Docker
- striving to have 'Magic numbers' removed from code for better overall clarity

## How to make this code base production ready?

- basic ApiKeyAuth middleware added, but it will need a proper auth
- additional logging from Controller to services and validators for easier debugging (this implementation only has some simple logging setup for demo purposes)
- extend ShoppingBasketValidator and add tests
- add more tests around ShoppingBasketService
- connect to database and implement Persistence for it with (DbContext, data seeding, migrations, caching etc.)
- hook into pipeline