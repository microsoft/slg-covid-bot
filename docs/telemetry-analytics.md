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
