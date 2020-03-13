# People API Documentation

## Running the API

### Prerequisites

The People API uses dotnet core version 3.1. Make sure that you have that version installed before you proceed.

### Requirements

The requirements for the take home web project are as follows:
- It needs to be written in Angular 2+
- It needs to display a search box where a user can search for people to display in a list
    - The search term should include users where any part of their name matches, e.g. a search term of "bob" would return the user "Jimbob Cucumber"
    - Display the list in a pleasing manner where we can see all of the user's information
- It needs to provide the ability to create and edit users
- It needs to provide the ability to slow down the network requests and have the UI handle the delay gracefully

In addition, the app needs to have any 2 of the following:
- [Reactive forms](https://angular.io/guide/reactive-forms)
- [NgRx](https://ngrx.io/)
- [Cashmere](https://cashmere.healthcatalyst.net/)

**Changes to the backend code are not allowed!**

### Starting the server

Extract the PeopleApi.zip file. Make sure you keep all extracted files intact. From the extracted directory, run
`dotnet assessment.dll`. You should see output similar to the following:

```
Seeding database...
Database seeded.
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\...
```

.NET Core can be downloaded here: https://dotnet.microsoft.com/download

### Accessing the API

From a web browser, go to https://localhost:5001/odata/People. You should see a lot of JSON returned:

```json
{"@odata.context":"https://localhost:5001/odata/$metadata#People","value":[{"Id":1,"Gender":"female","NameSet":"Russian","Title":"Ms.","GivenName":"Jasmine","MiddleInitial":"A","Surname":"Kudryashova","StreetAddress":"8 Magnolia Drive","City":"BEXLEY SOUTH","State":"NSW","StateFull":"New South Wales","ZipCode":"2207","Country":"AU","CountryFull":"Australia","EmailAddress":"JasmineKudryashova@dayrep.com","Username":"Searturefor72","Password":"Ooghouk9thee","BrowserUserAgent":"Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.103 Safari/537.36","TelephoneNumber":"(02) 6288 3547","TelephoneCountryCode":61,"MothersMaiden":"","Birthday":"1972-11-02T00:00:00-07:00","Age":47,"TropicalZodiac":"Scorpio","CCType":"Visa","CCNumber":"4532347321176145","CVV2":"598","CCExpires":"2/2021","NationalID":"","UPS":"1Z 5V5 6W8 09 9116 335 2","WesternUnionMTCN":"0929612979","MoneyGramMTCN":"63241625","Color":"Purple","Occupation":"Multimedia artist","Company":"Disc Jockey","Vehicle":"1998 Volvo S90","Domain":"ToothSearch.com.au","BloodType":"B+","Pounds":210.1,"Kilograms":95.5,"FeetInches":"5' 2\"","Centimeters":158,"GUID":"09f6a49d-4e3a-46ab-80cc-8ce963b26ce5","Latitude":-34.042673,"Longitude":151.119338},{"Id":2,"Gender":"female","NameSet":"Hobbit","Title":"Mrs.","GivenName":"Mantissa","MiddleInitial":"G","Surname":"Brockhouse","StreetAddress":"Via Domenico Morelli ...
```

CORS is enabled, so you shouldn't have any problems accessing this API from your web application.

## API methods

### `https://localhost:5001/odata/$metadata`

Displays the structure of the `Person` class, which you can use to create your client's data model type.

### `GET https://localhost:5001/odata/People`

Gets all people. Supports OData query operators (see <https://skyvia.com/blog/odata-cheat-sheet>).

### `GET https://localhost:5001/odata/People(ID)`

Gets person with specified `ID`, i.e. `GET https://localhost:5001/odata/people(7)`.

### `POST https://localhost:5001/odata/People`

Add a new person to the collection. Requires a `Person` object in the body of the request.

### `PUT https://localhost:5001/odata/People(ID)`

Updates the person with specified `ID`. Requires a `Person` object in the body of the request.

### `DELETE https://localhost:5001/odata/People(ID)`

Deletes the person with specified `ID`.
