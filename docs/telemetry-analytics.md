Telemetry and Analytics 
=======================

App Insights (Time to Setup -- 3 Minutes)
-------------------------------------------------------

Q&A Maker can be deployed with **App Insights** to capture an analyze
application telemetry. In the next few steps you'll learn how to monitor
these metrics, including Questions, Answers and Confidence Scoring. By
the end of this section you'll be able to create the following report:

    ![](.//media/image24.png)

Please refer to this link for more information:
<https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/how-to/get-analytics-knowledge-base>

1.  Locate **Application Insights** you've created when initially
    deploying the bot

    ![](.//media/image25.png)

2.  Check that it's collecting data, you should see charts like these,
    if you don't the App Insights aren't properly configured.

    ![](.//media/image26.png)

3.  Click on Logs at the top

    ![](.//media/image27.png)

4.  Run a Kusto query (copy and paste) from this
    [Document](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/how-to/get-analytics-knowledge-base)
    -- Step 3

    ![](.//media/image28.png)

5.  Done.

Automatic email notification of unanswered questions (Time to Setup -- 10 Minutes)
----------------------------------------------------------------------------------

1.  Create a new **Logic App** in **Azure**. We recommend putting it
    into the same **Resource Group** as other Bot resources

    ![](.//media/image29.png)

2.  Once created, click on **Blank Logic App** and select **Schedule**
    as the trigger

    ![](.//media/image30.png)

3.  Set the **Recurrence** to 1 hour

    ![](.//media/image31.png)

4.  Click on **+ New Step** and select **Visualize Analytics query**

    ![](.//media/image32.png)

5.  Give it a **Connection Name** of your preference

6.  The **Application ID** will be located in **Application Insights** -
    \> **API Access**

    ![](.//media/image33.png)

7.  Now **Create API key** to allow your **Logic App** to authenticate

    ![](.//media/image34.png)

8.  API Key Configuration

    ![](.//media/image35.png)

9.  Go back to **Logic App Designer** and fill in the **Application ID**
    and **API Key**

    ![](.//media/image36.png)

10. Now configure the **\*Query** ([copy and paste from
    here](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/how-to/get-analytics-knowledge-base#unanswered-questions))

    ![](.//media/image37.png)

11. Add **+ New Step** to email the results to specified people

    ![](.//media/image38.png)

12. Authenticate and fill in the appropriate fields

    ![](.//media/image39.png)

13. Click **Save** and **Run.** Check email, you should receive an email
    like this one

    ![](.//media/image40.png)

14. Done.

Visualizing, grouping and analyzing customer interactions (Time to Setup -- 10 Minutes)
---------------------------------------------------------------------------------------

1.  Pre-Requisite - [Download Power BI
    Desktop](https://www.microsoft.com/en-us/download/details.aspx?id=58494)
    (free)

2.  Download and Open [Power BI Report Template](https://github.com/microsoft/slg-covid-bot/raw/master/resources/COVID19%20Bot%20Dashboard.pbix)

    ![](.//media/image41.png)

3.  Edit Parameters to update local time **UTC Offset** and
    **Application Insights ID** (from previous section, step 6)

    ![](.//media/image42.png)
>
    ![](.//media/image43.png)
>
    ![](.//media/image44.png)

4.  **Log In** using **Organizational account**

    ![](.//media/image45.png)

5.  Done
