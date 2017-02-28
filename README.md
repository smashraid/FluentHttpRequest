Fluent Http Request
===================


This library is created to help you building request for GET and POST

----------


Getting Started
-------------

Use the GUI or the following command in the Package Manager Console

    Install-Package FluentHttpRequest


----------

How to use it
-------------------
This will return the response as string

    var request = RequestBuilder
                 .Create("https://your-url.com")
                 .Get();

If you want to call in async way you can use GetAsync

    var request = RequestBuilder
                 .Create("https://your-url.com")
                 .GetAsync();
		 
 ----------


Parameters
-------------

     var request = RequestBuilder
                 .Create("https://your-url.com")
                 .AddParam("key1", "value1")
                 .AddParam("key2", "value2")
                 .Get();

This will generate a url https://your-url.com?=key1=value1&key2=value2

----------

Body Parameters
-------------

     var request = RequestBuilder
                 .Create("https://your-url.com")
                 .AddBodyParam("key1", "value1")
                 .AddBodyParam("key2", "value2")
                 .Post();

----------


Map result as a class
--------------------

    public class User {
	    public int Id { get; set; }
	    public int Name { get; set; }
    }
Then you need to create a RequestBuilder

    var request = RequestBuilder
                 .Create("https://your-url.com")
                 .Get()
                 .Fill<IList<User>>();

----------


Add Header
--------------------

	var request = RequestBuilder
	              .Create("https://your-url.com")
	              .AddHeader("ke1", "value1")
	              .AddHeader("ke1", "value1")
	              .Get();
		      
		      
