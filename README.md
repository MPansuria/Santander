The solution is prepared using .NET Core 8
Required Nuget Packages - 
  Automapper 12.0.1 
  Swashbuckle.AspNetCore 6.4.0

How to run -
The code can run using VS2022. Swagger is integrated which should load swagger page on running the application through which the API endpoint can be called.

Code & functionality -
Controller - HackerNewsStoriesController implements GetBestStories endpoint to retrieve the details of the best n stories from the Hacker News API, in descending order of their score.
Calls to retrieve details are invoked in parallel to improve performance. ConcurrentBag is used to save results in a threadsafe way.
Max number of parallel request (configured through appsettings) will give control on not overloading the Hacker News API with too many calls simulteneously. 

AutoMapper is used to data into the form required.

Assumptions -
Hacker API best stories endpoint only gives 200 stories. It is assumed that 200 is the max number of stories that this API will return even if user has passed a greater number as parameter.

What enhancements can be made -
Rate limiting (throttling) can be implemented to prevent spike in requests slowing down the performance or overloading the Hacker News API.
