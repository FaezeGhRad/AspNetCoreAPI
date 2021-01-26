# Introduction 
This is an HTTP REST API that exposes, saves, updates and deletes JSON objects in a secure way based on user’s credentials .<br/> 

# Implementation Summary 
I used .NetCore 3.1 and .NetStandard 2.0 <br/>
I follwed a simple pattern and created API , Abstractions and Implementations projects, those could indeed get further narrowed to different logical layers (e.g. data , services ,entities and etc) in separate projects.<br/>
<br/> 
- Abstractions project includes :<br/>
• Data Entities (Entity and User)<br/>
I defined an Entity type contains a key, creator and a string field to store json content. I also created a User type to keep users’ info like username and password.<br/>
• Repository interfaces to handle CRUD for mentioned entities. <br/>
• Service interfaces : <br/>
IAccountService : responsible for account registration and authentication<br/>
IDataProtectionService : responsible for data encryption , decryption , hashing and validation. This is used for json object encryption and decryption as well as user’s password hashing and validation.<br/>
<br/>
- Implementations project contains :<br/>
• DataProtectionService: Implements IDataProtectionService using symmetric ASE algorithm and a third party library to hash and verify.<br/>
• AccountService: Implements IAccountService and requires IUserRepositoty instance to interact with db and needs IDataProtectionService implementation to hash and verify passwords. <br/>
• Repository implementations that uses EFCore (and Sqlite) to persist the data <br/>
I decided to encrypt and decrypt the json in data layer inside EntityRepository, it takes the determined private key from authenticated user’s claims and uses it for encryption and decryption via an IDataProtectionService instance. <br/>
 <br/>
<br/>
- API project has:<br/>
• Host, DI, services and Swagger configurations.<br/>
• Models (DTOs) for entity and also user registration and authentication.<br/>
• Mappers (Entities to DTOs).<br/>
• Controllers :<br/> 
EntitiesController that uses an IEntityRepository instance to store and retrieve entities.<br/>
UsersController that uses an IUserRepository instance to register and retrieve users.<br/>
• AuthenticationHandler: <br/>
I created BasicAuthenticationHandler that needs an IAccountService implementation to authenticate the user and set necessary claims such a new custom claim as [PrivateKey] which is a combination of the authenticated user’s username and password. <br/>
This claim later is used to encrypt and decrypt the json pbject.<br/> 
• JsonConverter :<br/> 
I created a EntityJsonConverter in order to intercept deserialization before the response is sent and to append a new field as “_id” to the json object , it helps consumer to identify the returned object.<br/> 
 <br/>
<br/>
- Test project includes testing for services 

# Build and Run
Build and run the soltion via VisualStudio or use dotnet core CLI <br/>
The root page should show Swagger where you can use the API <br/>

# Further Improvements
I’ve time boxed doing developing of this application, so I’ve not implemented everything needed for a production ready application.<br/>
If I have a chance to further continue, I would go for the following tasks/features: <br/>
• Enhance logging and errors/messages localization.<br/>
• Enhance exception handling, e.g. consistent message format between controller and handler middleware.<br/>
• Enhance tests, although important logics covered in service tests but I haven’t yet added tests for controllers!<br/>
• Introduce API versioning, configuring CORS and more advanced configurations.<br/>
• Small cleanup and refactoring like finalizing TODOs.<br/>
<br/>
