# ASP.NET Web API Server Project of BizBook365 ERP
The server repository of bizbook server.

## Professional help and support
Email: foyzulkarim@gmail.com

## Have question? Community is here
Please let us know if you have any Question / Confusion so that we can add them in readme or Wiki or make videos to help you appropriately. 
Please visit our facebook page [Code Coopers Academy](https://www.facebook.com/codecoopersacademy) and join in our [group](https://www.facebook.com/groups/codecoopersacademy/) for active participation.

## Installing

A step by step series of examples that tell you how to get a development env running.

1. Clone the repository
2. Go to the BizBook directory
3. Open `BizBook.sln` file using Visual Studio 2017
4. Open Package Manager Console
4. Run `Update-Package -Verbose` to install required dependencies.
5. From above `Default project` dropdown, select `Server.Identity` project and run `Update-Database -Verbose` [Watch below video for details]
6. From above `Default project` dropdown, select `Model` project and run `Update-Database -Verbose` [Watch below video for details]
7. Go to sql folder of the repository and execute `001_Permission_Seed_Data_Insert.sql` and then `002_SuperAdmin_Data_Insert.sql` against your newly created `BizBookDb` database. [Watch below video for details]

8. For rest of the step, please follow this video. [Check video instruction](https://youtu.be/uQzsSb2Nl-8)

### Demo images

![Sales list page](resources/images/bizbook-sales-cover.PNG)

### Deployment

[Details will be added soon]

## Technologies used / Built with
I have used .NET Framework 4.6.2 along with,
1. ASP.NET Web API 
2. Entity Framework
3. SQL Server 2017 Express 


## Contributing

Currently I am not taking any external contribution. I will make it open for contribution after I make the repository ready. But I will be very happy to see your support by marking a Star and Fork this repository. 


## Versioning

We will use [SemVer](http://semver.org/) for versioning. 

## Authors

* **Foyzul Karim** - *Initial work* - [Foyzul Karim](https://github.com/foyzulkarim)

## License

This project is licensed under the GNU GENERAL PUBLIC LICENSE - see the [LICENSE.md](https://github.com/foyzulkarim/bizbook-angularjs-ts-client/blob/master/LICENSE) file for details


