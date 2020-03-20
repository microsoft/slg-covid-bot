# Downloading source code to make a customized bot

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
