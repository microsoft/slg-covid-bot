Requirements
============

-   Azure Commercial Subscription

-   Visual Studio 2019

-   Basic experience working within the Azure Portal

Deployment
==========

1.  Download the sample code and open in Visual Studio 2019.

2.  Review and make any code changes if desired.

3.  Right click on the project and choose publish.

4.  Choose Azure Functions Consumption Plan in the left panel.

5.  Choose Create New in the right panel.

6.  Ensure that Run from package file is checked.

![](.//media/image1.png){width="3.2922080052493436in"
height="2.4705632108486437in"}

7.  Click on the Create Profile button

8.  Provide a name for the function app. This must be globally unique.

9.  Choose your Azure Subscription.

10. Create a new Resource Group, or use an existing one if desired.

11. Choose your Azure Region.

12. Choose an Azure Storage Account, or create a new one if desired.

![](.//media/image2.png){width="3.168830927384077in"
height="2.3766229221347333in"}

13. Click on the Create button.

Configuration
=============

The Azure Function uses configuration variables. The list below outlines
the variables that need to be configured:

-   kbid -- The unique QnA Maker Knowledgebase ID.

-   apiKey -- API key for managing the Knowledgebase.

-   botEndpoint -- URL for the Azure Bot endpoint.

-   questionId -- A unique question ID located in the specified KB. The
    answer for this question will be automatically updated when the
    function runs.

-   state -- The full name of the state as listed in the John Hopkins
    data source.

To set the configuration values:

1.  Open the Azure Portal and locate the Function App that you deployed
    in the prior section.

2.  Click on the Function App Name in the left panel.

3.  In the main panel, click on the Configuration link.

![](.//media/image3.png){width="4.253246937882765in"
height="1.837166447944007in"}

4.  Add new application setting for each of the configuration variables
    listed above. Ensure that you are setting the values appropriately
    for your QnA Maker Knowledgebase. The configuration name must
    exactly match the name shown above.

Test Run
========

By default the function will run on an hourly basis. If you wish to do a
test run of the function:

1.  Open the Azure Portal and locate the Function App that you deployed.

2.  Click on the CovidDataImport function located under the Functions
    section in the left panel.

3.  Click on the Run button.

4.  As the function runs, watch the output log at the bottom of the page
    to ensure no errors are shown. If errors are shown, look in the log
    and verify that all of the configuration values are correct. If they
    are not correct, return to the Azure Function configuration and
    update the values.

Locating the values used for configuration.
===========================================

kbId
----

The kbId value can be located in the URL when viewing your QnA Maker
Knowledgebase. For example:

![](.//media/image4.png){width="5.48701334208224in"
height="0.23331583552055993in"}

Would indicate a kbId of a151c1e2-b547-41d5-b92c-003c4043dab0

apiKey
------

An API Key for your Azure Bot Service needs to be generated so the Azure
Function has permissions to update the knowledgebase question.

1.  Open the Azure Portal and go to the Cognitive Services that was
    created for your bot. The apiKey value you need to set in the Azure
    Function configuration is listed as Key1 in the Quick Start page for
    your Cognitive Services.

> ![](.//media/image5.png){width="3.6815977690288713in"
> height="0.9350645231846019in"}

botEndpoint
-----------

1.  Open the Azure Portal and go to the Cognitive Services that was
    created for your bot. The botEndpoing value you need to set in the
    Azure Function configuration is listed as Endpoint in the Quick
    Start page for your Cognitive Services.

> ![](.//media/image5.png){width="3.6815977690288713in"
> height="0.9350645231846019in"}

questionId
----------

The questionId is the unique question ID in the knowledgebase that
should have it's answer updated. Getting this ID requires making a call
to a QnA Maker API endpoint. To do this we will use a free tool called
Postman (<https://www.postman.com/downloads/>)

1.  Download, install and open the Postman application.

2.  In the request dropdown choose Get

3.  In the box to the right of the dropdown enter in the following URL,
    replacing the {tokens} shown below with actual values from the
    variables we previously identified.

{botEndpoint}/knowledgebases/{kbId}/prod/qna

When entered, it will look something like:

> ![](.//media/image6.png){width="4.324675196850394in"
> height="0.517020997375328in"}

4.  Click on the Headers link below the URL box.

5.  In the first row for the headers enter in a key of
    Ocp-Apim-Subscription-Key.

6.  Set the value for the Ocp-Apim-Subscription-Key to the apiKey value
    identified earlier. Once completed it should look similar to:

> ![](.//media/image7.png){width="4.493506124234471in"
> height="1.2659590988626421in"}

7.  Click on the Send button. If everything was set correctly you should
    see a JSON formatted document shown in a panel at the bottom of the
    application. You will notice that every question in your
    knowledgebase is listed. Each question and answer pair starts with
    an id. Locate the question you wish the Azure Function to update and
    note down the id. This will be the questionId used in your Azure
    Function configuration.

> ![](.//media/image8.png){width="4.53246719160105in"
> height="2.2928663604549433in"}
