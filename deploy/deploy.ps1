$location = "westus2"

az deployment sub create `
	--location $location `
	--template-file .\deploy\Main.bicep `
	--parameters .\deploy\parameters.dev.json  `
	--confirm-with-what-if