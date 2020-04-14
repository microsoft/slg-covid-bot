# This PowerShell script creates and populates a kb, then tests responses from the kb
$QnACogServiceKey = "<YOUR_QNA_KEY_HERE>" 
$kbAppServiceEndpoint = "<YOUR_KB_ENDPOINT_HERE>"
$kbName = "<YOUR_KB_NAME_HERE>"

# Add headers for API calls
$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Content-Type", "application/json")
$headers.Add("Ocp-Apim-Subscription-Key", $QnACogServiceKey)

# This body is used to populate the kb, add/replace urls to the urls array as desired
$body = "{`"name`": `"$kbName`",`"urls`": [`"https://www.cdc.gov/coronavirus/2019-ncov/faq.html`"]}"

# Create the kb
$createResponse = Invoke-RestMethod 'https://westus.api.cognitive.microsoft.com/qnamaker/v4.0/knowledgebases/create' -Method 'POST' -Headers $headers -Body $body
$createResponse | ConvertTo-Json

# kb creation is a long running operation, use the operation id to check creation status. by default, checks every fifteen seconds to avoid API rate limiting
Do {
Write-Host "waiting to check status of create operation..."
Start-Sleep -s 15
$createStatusResponse = Invoke-RestMethod "https://westus.api.cognitive.microsoft.com/qnamaker/v4.0/operations/$($createResponse.operationId)" -Method 'GET' -Headers $headers
$createStatusResponse | ConvertTo-Json
}
While ($createStatusResponse.operationState -eq "NotStarted" -or $createStatusResponse.operationState -eq "Running") 

# if kb creation was successful, queary/generate answer from the kb
If ($createStatusResponse.operationState -eq "Succeeded")
{
    # publish the kb, investigate invoke-webrequest to catch 204 (request processed, but no response code)
    $publishResponse = Invoke-RestMethod "https://westus.api.cognitive.microsoft.com/qnamaker/v4.0/$($createStatusResponse.resourceLocation)" -Method 'POST' -Headers $headers
    $publishResponse | ConvertTo-Json

    # get an endpoint key to query the kb
    $runtimeKeyResponse = Invoke-RestMethod 'https://westus.api.cognitive.microsoft.com/qnamaker/v4.0/endpointkeys' -Method 'GET' -Headers $headers
    $runtimeKeyResponse | ConvertTo-Json

    # adjust headers for api call against the kb
    $headers.Remove("Ocp-Apim-Subscription-Key")
    $headers.Add("Authorization", "EndpointKey $($runtimeKeyResponse.primaryEndpointKey)")

    # ask question, show answer
    $question = "{`"question`": `"Should I wear a mask?`"} "
    $questionResponse = Invoke-RestMethod "https://$kbAppServiceEndpoint/qnamaker$($createStatusResponse.resourceLocation)/generateAnswer" -Method 'POST' -Headers $headers -Body $question
    $questionResponse | ConvertTo-Json
}