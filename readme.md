This application enables you to send TFS commit/changeset info to Wrike automatically upon commit, so that when you do this:

![image](https://user-images.githubusercontent.com/2829865/37444926-9905d87c-2868-11e8-9f07-d9490d8a0d22.png)

You will see this:

![image](https://user-images.githubusercontent.com/2829865/37444763-8e2ded82-2867-11e8-9e75-a29070fdc6f9.png)

This is done by implementing a TFS service hook, so this is a WebApi project. Upon receiving the hook call from TFS, it will update Wrike with the details of the commit using the Wrike API. 

To accomplish this, download this project, compile and host it somewhere in IIS under a virtual directory named something like "api-wrike-updater". Assuming your server name is "MyAwesomeHost", you should be able to access the following:

http://MyAwesomeHost/api-wrike-updater/v1/hello-world

which should spit back "Hello World". 

Great. Now open Wrike, click on your profile, then go to Apps & Integrations -> API -> Create new. Give it a usefulname like "TFS Hook" and create a Permanent access token. Now edit the web.config file and put this token into the Wrike:oAuth key. You should now be able to access this URL:

http://MyAwesomeHost/api-wrike-updater/v1/custom-fields

which should display a list of all of your custom fields within Wrike. If you haven't already, create a new custom field named "CommitId" (if you're using Git) and another named "ChangesetID" (if you're using TFVC) - or create both if you're using both. Go back to the custom-fields page, refresh, and you should find the IDs for the custom fields. Paste these into the Wrike:CommitCustomFieldID key and Wrike:ChangesetCustomFieldID keys.

Now open TFS, create a new service hook, and add one for "Code checked in" and point it towards this URL:

	http://MyAwesomeHost/api-wrike-updater/v1/code-checkin-hook

click "Test" and it should succeed. Now do another for "Code pushed" and point it towards this URL:

	http://MyAwesomeHost/api-wrike-updater/v1/code-pushed-hook

Click "Test", and it should succeed.

If you now commit some code with a message that looks like this:

	"Updated code because of reason x, Wrike 12345123"

It will update task 12345123 with the changeset ID, commitID and also post a comment with a link to the commit. You will need within Wrike to display the custom fields for that task (you can do this in the table view).
