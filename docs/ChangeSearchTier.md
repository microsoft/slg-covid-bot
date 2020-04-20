Switch Azure Cognitive Search Tier for QnA Maker Bot
====================================================

When you create an Azure Cognitive Search service, a resource is created at a pricing tier (or SKU) that's fixed for the lifetime of the service. If you need to change to a differnt tier you will need to create a new search service instance at the proper tier level and then migrate the indexes to the new location. Below outlines the steps 

Steps
-----

1.	Create a new search service in the correct region.  Choose the tier you would like to use.

2.	Clone the code from https://github.com/pchoudhari/QnAMakerBackupRestore and use the AzureSearchBackupRestore project to migrate the index from the old search service to the new one.  This requires Visual Studio 2017 or higher.  In the program.cs file you will need to update the code to specify the source and destination search names and API keys.

3.	Follow the steps shown https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/how-to/set-up-qnamaker-service-azure#configure-qna-maker-to-use-different-cognitive-search-resource to point to the new search service.  

4.	Test to ensure your bot is working correctly.

5.	Delete the original search service
