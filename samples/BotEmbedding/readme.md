# Bot Embedding

Paste this inline HTML code on the page where you want the chat bubble to appear.

## Style Setting options

Adjust style settings as appropriate for your site:

* **Height/width:** The height and width of the iframe can be adjusted to fit your site.
* **Right/bottom:** Both the image and iframe have fixed positions and the "right" and "bottom" style settings set the element's positions relative to the bottom-right corner of the screen. Put the iframe 98px above the image (iframe bottom = img bottom + img height + 10px).
* **Z-index:**(( Set the z-index for both the image and the iframe so they are above any other page content.
* **Max-width:** The max-width value you see on the iframe works to keep the whole element visible on mobile displays in portrait mode when the position-right is 18px.  Adjust this if needed so the iframe doesn't overflow the screen on mobile displays.
* **Max-height:** The max-height value on the iframe works to prevent the top of the iframe from overflowing the top of the window. The value of 188px less than 100% works if the iframe bottom is set to 178px.  Adjust as necessary so the iframe doesn't overflow the top of the screen on mobile displays.

> **NOTE:** The placement of the image and iframe in this sample allows room for a return-to-top link that appears in the bottom-right corner of the window as the user scrolls down the page. Adjust as appropriate for your page's layout.
