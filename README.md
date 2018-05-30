# yellowant-dotnet-app

Sample dotnet application for creating a Hello application for YellowAnt

## Getting Started

These instructions will get you started with building basic YellowAnt application.

## Register this application with your YellowAnt Developer account

1. Once you have logged into YellowAnt, head over to your team's subdomain developer page, <https://your-team-subdomain.yellowant.com/developers/>

2. Click on the button "Create New Application"

3. Fill the form and click on "Create Application":
![YellowAnt Create New App](https://github.com/yellowanthq/yellowant-sample-django-app/blob/master/docs/yellowant-create-new-app.jpg "YellowAnt Create New App")
    - Display Name: A human readable display name for the application.
    - Invoke Name: A simple single word which users can use to control this app.
    - Short Description: A human readable short description

4. After the application is created you will be at the application overview page. You need update the application with more information and click on "Update Application".
![YellowAnt Update App](https://github.com/yellowanthq/yellowant-sample-django-app/blob/master/docs/yellowant-app-overview-1.jpg "YellowAnt Update App")
![YellowAnt Update App](https://github.com/yellowanthq/yellowant-sample-django-app/blob/master/docs/yellowant-app-overview-2.jpg "YellowAnt Update App")
    - API URL: The endpoint through which YellowAnt will communicate with this app.
    - Installation Website: The URL of your app where users will be able to begin integrating their YellowAnt accounts with this app.
    - Redirect URL: The endpoint at which YellowAnt will send the OAuth codes for user authentication.
    - Icon URL: A URI which points to an icon image for this app.
    - Creator Email: Your Email.
    - Privacy Policy URL: Any policy or TOC URL for your app.
    - Documentation URL: A documentation website URL for your app.
    - Is Application Active: set to "Active".
    - Is Application Production or Testing: set to "Production".
    - Application Visibility: set to "Public".

5. You need to create the 5 functions that are understood by this Django app.
    1. createitem(title, description): create a new todo item
        - title [varchar, required]: title of a todo item
        - description [varchar]: extra details of a todo item
    2. getlist(): get a list of todo items
    3. getitem(id): get a single todo item
        - id [int, required]: id of the todo item
    4. updateitem(id, title, description): update a todo item
        - id [int, required]: id of the todo item
        - title [varchar]: new title of the todo item
        - description [varchar]: new description for the todo item
    5. deleteitem(id: int): delete a todo item
        - id [int, required]: id of the todo item

![YellowAnt Create New Function](https://github.com/yellowanthq/yellowant-sample-django-app/blob/master/docs/yellowant-create-new-function.jpg "YellowAnt Create New Function")
![YellowAnt Create New Input Arg](https://github.com/yellowanthq/yellowant-sample-django-app/blob/master/docs/yellowant-create-new-arg.jpg "YellowAnt Create New Input Arg")
```
Example of how to create the function, createitem, which has two input arguments, title and description:

1. Click on "Add New Function".
2. Complete the form and click on "Create New Function":
    - Display Name: Human readable name for this function. e.g. "Create a Todo Item"
    - Invoke Name: A simple descriptive word for invoking this command. e.g. "createitem"
    - Description: Description. e.g. "Add a new item to your todo list"
    - Function Type: Set to "Command"
    - Is Function Active: Set to "Yes"
3. After creating a new function, you're at the function overview page, scroll down to the section for input arguments, and click on "Add New Input Arg".
4. Add a new input argument, title, and click on "Save":
    - Display Name: A simple description word for this argument. e.g. title
    - Description: Describe the use for this argument. e.g. A title which summarizes this todo
    - Type: The data type of this argument. e.g. varchar
    - Required: Toggle it on.
    - Input Example: A human readable example. e.g. Get Milk
4. Add a new input argument, description, and click on "Save":
    - Display Name: A simple description word for this argument. e.g. description
    - Description: Describe the use for this argument. e.g. Details about the todo item
    - Type: The data type of this argument. e.g. varchar
    - Required: Toggle it off.
    - Input Example: A human readable example. e.g. Get non-skimmed milk from Krogers at 4th Cross St.
```

## Getting started with application
This application helps you start with writing application code. When going to production, make sure you load all sensitive 
tokens and values through environment variables or encoded secrets for better security.

1. Open solution in Visual Studio. Make sure you have installed YellowAntSDK from nuget package manager.
2. Copy ClientID, ClientSecret, Veirifcation token from your Yellowant dashboard to relevant sections in 
Controllers/UserIntegrationController.cs 
3. Start development server by clicking on Debug(your-default-browser) button or by striking (Ctrl + F5)
4. This will open a window in your browser with ```localhost:port```
5. Note down the port number. you might need this to run ngrok and make your application available for testing on 
production

### Using Ngrok
Ngrok provides public URLs for your apps on local machine. You can use this to test out your application before launching 
it in production. Head over to [Ngrok](https://ngrok.com/) and create an account. Follow the instructions to set up ngrok 
on your machine. ngrok server by using 

```ngrok --host-header localhost:<port-your-servers-running-on>  http <port-your-servers-running-on>```

```<port-your-servers-running-on>``` is the same port from above.

After you start ngrok, note the link. 
1. You need to update your app Redirect URL and API URL in Yellowant dashboard. 
2. Update Redirect URI in Controller/UserIntegrationController ```oauthredirect``` function. Also Update Redirect(<ngrok url>)
 
Now you should be ready to communicate with Yellowant.

### Using RTM Socket 
In case your development server is behind a firewall, you can use yellowant socket server to communicate.
1. Enable RTM for your application on YellowAnt developer dashboard. Don't forget to click 'Update Application'
2. Download <yellowant rtm-client> and follow instructions to run socket client on your machine.  
3. Go to Controllers/UserIntegrationController.cs, in "API" method, comment the lines under 'NOT using RTM' and uncomment
the lines under "using RTM"
4. Start your development server

## Understanding Application Layout
This is an example application, to let you know basics of building an integration for YellowAnt. Don`t use this code for production.
There are two main components to notice
1. Controller (UserIntegrationController)
2. CommandCenter
  
### Controller
There are 4 controller functions Integrate, NewIntegration, Oauthredirect, and API. 

#### Integrate
When you start a server go to ```<ngrok-server>/userintegration/integrate```. If you are not logged in/signed up, 
this will redirect you to login/signup page. This is handled by Integrate controller.

#### NewIntegration 
Once on ```/userintegration/integrate```, click on 'Link Button'. This will redirect you to /userintegration/newintegration. In this 
controller, a user `state` is created and redirect link to yellowant is constructed using state and clientID. 

#### Oauthredirect 
If user approves request, YellowAnt will redirect you to 'redirect url' you have mentioned in application dashboard. This controller
handles this redirected request. Request comes with `state` you created in 'NewIntegration' and a 'token'. You use these 
and complete OAuth cycle to get secret token form YellowAnt.

#### API 
When user enters command in Slack/YellowAnt, that request is sent to 'API url' you mentioned in YellowAnt application dashboard.
Those requests are handled by this controller 

