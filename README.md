Fluent Http Request
===================


This library is created to help you

----------


Getting Started
-------------

Use the GUI or the following command in the Package Manager Console

    Install-Package FluentHttpRequest


----------

How to use it
-------------------

    var request = RequestBuilder
                 .Create("https://your-url.com")
                 .Execute();

----------


Parameters
-------------

     var request = RequestBuilder
                 .Create("https://your-url.com")
                 .AddParam("key1", "value1")
                 .AddParam("key2", "value2")
                 .Execute();

This will generate a url https://your-url.com?=key1=value1&key2=value2

----------


Map result as a class
--------------------

    public class User {
	    public int Id { get; set; }
	    public int Name { get; set; }
    }
Then you need to create a 	RequestBuilder

    var request = RequestBuilder
                 .Create("https://your-url.com")
                 .Execute()
                 .Fill<List<User>>();

----------


Add Header
--------------------

	var request = RequestBuilder
	              .Create("https://your-url.com")
	              .AddHeader("ke1", "value1")
	              .AddHeader("ke1", "value1")
	              .Execute();