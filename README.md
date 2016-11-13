# CrimeCluster2
Final Year Project - A Real Time Smart Police Dispatching System predicts crime clusters and provides a mechanism for importing new data. 

Solution breakdown: 

com.kiranpatel.crimecluster.dataaccess: Encapsulates all access to the database (Repository Pattern)

com.kiranpatel.crimecluster.framework: Encapsulates all business logic containing services, entities, enums, extensions. 

com.kiranpatel.crimecluster.framework.tests: Encapsulates unit tests for com.kiranpatel.crimecluster.framework. 

com.kiranpatel.crimecluster.importer: Encapsulates the CSV importer for reading historic crimes. 

com.kiranpatel.crimecluster.framework.predictor: Encapsulates prediction aspects of the application. 

com.kiranpatel.crimecluster.webservice: Encapsulates JSON API entry point to access data on the server. (ASP.NET MVC) 

com.kiranpatel.crimecluster.webservice: Encapsulates unit tests for com.kiranpatel.crimecluster.webservice