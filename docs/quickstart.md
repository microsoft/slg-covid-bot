Summary
=======

Government agencies are finding that their call centers and email
inboxes are being flooded with questions from concerned citizens about
the COVID-19 pandemic. Microsoft is suggesting that by deploying a
simple QnA Maker Bot on your public facing websites you can potentially
reduce these volumes while ensuring accurate information is being shared
with the citizens.

Standard FAQ websites are a great start, but it can sometimes be
difficult for a person to find the information that they need. Even
standard web site search boxes are inefficient. By using a bot backed
with AI citizens will more quickly find the relevant information they
require.

Time Required
=============

To deploy a standard QnA Maker Bot will take less than 1 hour.
Additional time will be needed in order to add the necessary HTML to
your public facing website and to import the necessary questions and
answers.

Requirements
============

To get started building a QnA Maker Bot you will need an Azure
Commercial subscription. If you do not already have a subscription,
please reach out to your Microsoft account team immediately and they can
assist. For the basic QnA Maker you do not need to have developer
experience. You will need basic HTML experience to add the necessary
HTML snippet to the public facing website.

To get started, ensure you have contributor rights on your Azure
Subscription and then follow the instructions below.

Provisioning the QnA Maker Knowledgebase
========================================

1.  Go to <https://qnamaker.ai>

2.  Sign in with your Microsoft Azure Subscription login name and
    password.

3.  Click Create a knowledge base

> ![](.//media/image1.png){width="3.610389326334208in"
> height="1.2860083114610674in"}

4.  Click Create a QnA service

> ![](.//media/image2.png){width="3.8766229221347333in"
> height="1.5771555118110236in"}

5.  A new browser tab will open, and you will be directed to it. Fill
    out the information to create the QnA service in your Azure
    subscription. A sample is shown below. You should create a new
    resource group for the QnA Bot. Click Create when done.

> ![](.//media/image3.png){width="4.803957786526684in"
> height="5.776042213473316in"}

6.  Wait until deployment is done.

![](.//media/image4.png){width="4.558441601049869in"
height="1.558928258967629in"}

7.  Go back to the QnA Maker browser tab and click Refresh in Step 2.

> ![](.//media/image5.png){width="4.045454943132109in"
> height="2.771742125984252in"}

8.  Select your Azure Active Directory ID and Subscription name. Then
    select the new QnA service you created in step 5. It should show up
    in the dropdown.

> ![](.//media/image6.png){width="3.6712882764654418in"
> height="2.5475546806649167in"}

9.  Select Language.

> ![](.//media/image7.png){width="2.870130139982502in"
> height="0.581654636920385in"}

10. Name you KB in step 3.

> ![](.//media/image8.png){width="3.5974026684164477in"
> height="0.7875087489063867in"}

11. Populate your KB in step 4 (optional). Enter a URL from your website
    with Covid FAQ or use CDC's URL, which is -
    <https://www.cdc.gov/coronavirus/2019-ncov/faq.html>. You can skip
    this step if you will be manually entering your questions and
    answers instead of importing them from a file or URL.

> ![](.//media/image9.png){width="4.610389326334208in"
> height="2.3864676290463693in"}

12. Add additional URLs or Files (optional)

13. Add personality of Bot (optional but recommend picking one). This
    will add some basic responses when folks greet the bot.

> ![](.//media/image10.png){width="3.5064938757655293in"
> height="1.5528762029746281in"}

14. Click Create your KB in step 5. If you get a message about "no
    endpoints" after clicking create your KB, go back up to step 2 in
    the form and click the refresh buttons and re-select the proper
    items. It can take a few moments after the resources are created for
    the endpoints to be live.

> ![](.//media/image11.png){width="3.610389326334208in"
> height="1.2964227909011374in"}

15. Click Save and train in the top menu if you made changes to the Q&A
    pairs. Otherwise, skip to step 16.

> ![](.//media/image12.png){width="1.8961034558180228in"
> height="0.917095363079615in"}

16. Click Publish in the top menu.

> ![](.//media/image13.png){width="3.759739720034996in"
> height="0.9302941819772529in"}

17. Click the Publish button located in the upper right of the page.

> ![](.//media/image14.png){width="4.136363735783027in"
> height="1.53876312335958in"}

18. Click Create Bot.

> ![](.//media/image15.png){width="2.8603772965879264in"
> height="2.7467530621172354in"}

19. A new tab with the Azure Portal will open. Accept all defaults but
    create the Bot and its resources in a region closest to you. Create
    an App service plan closest to you as well. Click OK.

> ![](.//media/image16.png){width="5.824675196850394in"
> height="4.115232939632546in"}

20. Click the create button at the bottom of the page.

21. Monitor the Bot creation. (use the bell icon at the top right
    corner). When it is done, click Go to resource.

> ![](.//media/image17.png){width="3.9155839895013123in"
> height="2.5415813648293963in"}

22. Click Channels and then click Get bot embed codes.

> ![](.//media/image18.png){width="4.636363735783027in"
> height="1.8951629483814523in"}

23. Click "Click here to open the Web Chat configuration page \>"

> ![](.//media/image19.png){width="3.0779221347331585in"
> height="1.575790682414698in"}

24. Click Show to get the Secret keys, copy it to notepad.

> ![](.//media/image20.png){width="4.792208005249344in"
> height="1.5592060367454068in"}

25. Click Copy for the Embed code, and copy it to notepad as well.

> ![](.//media/image21.png){width="4.922077865266842in"
> height="2.5520384951881017in"}

26. Take the key and put it into the embed code where it says
    YOUR\_SECRET\_HERE. (ps: this is a fake key for illustration
    purposes).

> ![](.//media/image22.png){width="4.98701334208224in"
> height="0.42091207349081367in"}

27. Final embed code should look like this.

> ![](.//media/image23.png){width="6.5in" height="0.1423611111111111in"}

28. The \<iframe\> embed code is what you will insert in your web page
    source (or master page). Save it for now.

29. Click Done.

Telemetry and Analytics 
=======================

[App Insights (Time to Setup -- 3 Minutes)]{.smallcaps}
-------------------------------------------------------

Q&A Maker can be deployed with **App Insights** to capture an analyze
application telemetry. In the next few steps you'll learn how to monitor
these metrics, including Questions, Answers and Confidence Scoring. By
the end of this section you'll be able to create the following report:

![](.//media/image24.png){width="6.5in" height="2.214583333333333in"}

Please refer to this link for more information:
<https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/how-to/get-analytics-knowledge-base>

1.  Locate **Application Insights** you've created when initially
    deploying the bot

![](.//media/image25.png){width="5.078678915135608in"
height="2.973957786526684in"}

2.  Check that it's collecting data, you should see charts like these,
    if you don't the App Insights aren't properly configured.

![](.//media/image26.png){width="3.845238407699038in"
height="3.929045275590551in"}

3.  Click on Logs at the top

![](.//media/image27.png){width="4.25in" height="0.9376334208223972in"}

4.  Run a Kusto query (copy and paste) from this
    [Document](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/how-to/get-analytics-knowledge-base)
    -- Step 3

![](.//media/image28.png){width="4.675324803149606in"
height="2.695803805774278in"}

5.  Done.

Automatic email notification of unanswered questions (Time to Setup -- 10 Minutes)
----------------------------------------------------------------------------------

1.  Create a new **Logic App** in **Azure**. We recommend putting it
    into the same **Resource Group** as other Bot resources

> ![](.//media/image29.png){width="3.0in" height="1.951923665791776in"}

2.  Once created, click on **Blank Logic App** and select **Schedule**
    as the trigger

> ![](.//media/image30.png){width="3.0in" height="1.6705129046369205in"}

3.  Set the **Recurrence** to 1 hour

> ![](.//media/image31.png){width="3.0in" height="0.7586537620297463in"}

4.  Click on **+ New Step** and select **Visualize Analytics query**

> ![](.//media/image32.png){width="3.0in" height="1.512179571303587in"}

5.  Give it a **Connection Name** of your preference

6.  The **Application ID** will be located in **Application Insights** -
    \> **API Access**

> ![](.//media/image33.png){width="3.0in" height="1.737843394575678in"}

7.  Now **Create API key** to allow your **Logic App** to authenticate

> ![](.//media/image34.png){width="3.0in" height="1.7268908573928259in"}

8.  API Key Configuration

> ![](.//media/image35.png){width="2.999096675415573in"
> height="1.809423665791776in"}

9.  Go back to **Logic App Designer** and fill in the **Application ID**
    and **API Key**

> ![](.//media/image36.png){width="3.0in" height="1.8935903324584427in"}

10. Now configure the **\*Query** ([copy and paste from
    here](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/how-to/get-analytics-knowledge-base#unanswered-questions))

> ![](.//media/image37.png){width="3.0in" height="2.854808617672791in"}

11. Add **+ New Step** to email the results to specified people

> ![](.//media/image38.png){width="3.0in" height="2.161218285214348in"}

12. Authenticate and fill in the appropriate fields

> ![](.//media/image39.png){width="3.0in" height="1.6080129046369205in"}

13. Click **Save** and **Run.** Check email, you should receive an email
    like this one

> ![](.//media/image40.png){width="4.0in" height="2.7021369203849517in"}

14. Done.

Visualizing, grouping and analyzing customer interactions (Time to Setup -- 10 Minutes)
---------------------------------------------------------------------------------------

1.  Pre-Requisite - [Download Power BI
    Desktop](https://www.microsoft.com/en-us/download/details.aspx?id=58494)
    (free)

2.  Download and Open Power BI Report Template from
    <https://microsoft-gov.github.io/>

> ![](.//media/image41.png){width="2.9010990813648294in"
> height="1.703465660542432in"}

3.  Edit Parameters to update local time **UTC Offset** and
    **Application Insights ID** (from previous section, step 6)

> ![](.//media/image42.png){width="2.738410979877515in"
> height="1.1184765966754155in"}
>
> ![](.//media/image43.png){width="3.4161089238845146in"
> height="0.8740999562554681in"}
>
> ![](.//media/image44.png){width="2.7381944444444444in"
> height="1.2228827646544183in"}

4.  **Log In** using **Organizational account**

> ![](.//media/image45.png){width="2.8540988626421697in"
> height="1.3516480752405948in"}

5.  Done

**Appendix** 
------------

### 

### Downloading source code to make a customized bot

1.  Open up the Azure Portal and choose your Web App Bot resources.

2.  Select Build, then click Download Bot source code

> ![](.//media/image46.png){width="4.723905293088364in"
> height="2.2246773840769904in"}

3.  Select Yes to include app settings.

> ![](.//media/image47.png){width="3.662338145231846in"
> height="2.025241688538933in"}

4.  When download is ready, click Yes to download the file. This will
    download a zip file to your downloads folder.

![](.//media/image48.png){width="3.136707130358705in"
height="2.7532469378827646in"}

5.  Click Home at the top left corner.

> ![](.//media/image49.png){width="5.270833333333333in"
> height="4.739583333333333in"}

6.  Locate the Resource group for your Bot assets. It should be in the
    Recent resources list. Make sure you select the Resource group
    object type.

> ![](.//media/image50.png){width="4.568678915135608in"
> height="3.1028947944007in"}

7.  You should see all the assets required for your Bot. Click on the
    App Service object.

> ![](.//media/image51.png){width="4.257407042869641in"
> height="1.6888626421697288in"}

8.  Click Get publish profile. This will download a file to your
    downloads folder.

> ![](.//media/image52.png){width="3.6036756342957132in"
> height="2.0339982502187226in"}

9.  Copy the zip file (from step 4) from your downloads folder to a new
    directory (you can name that directory Covid).

10. Copy the publish profile (from step 8) from your downloads folder to
    the same directory as the zip file.

11. Unzip the contents of the zip file.

12. Launch Visual Studio and select Open a project or solution
    ([download and install Visual Studio 2019 Community
    Edition](https://visualstudio.microsoft.com/downloads/) if you do
    not have Visual Studio).

> ![](.//media/image53.png){width="4.579255249343832in"
> height="2.8571423884514435in"}

13. Open QnABot.sln.

> ![](.//media/image54.png){width="4.064935476815398in"
> height="2.3117147856517937in"}

14. Click Build -\> Publish QnABot.

> ![](.//media/image55.png){width="4.234686132983377in"
> height="3.5016229221347333in"}

15. Select Import Profile.

> ![](.//media/image56.png){width="3.5in" height="2.6620188101487314in"}

16. Pick the Publish profile you downloaded in step 4.

17. Click Publish.

> ![](.//media/image57.png){width="4.564935476815398in"
> height="2.259057305336833in"}

18. When your bot is successfully built and deployed, you will see this
    page.

> ![](.//media/image58.png){width="3.9354068241469817in"
> height="2.9675328083989503in"}

19. Close all browsers, save your Visual Studio project and exit Visual
    Studio.
