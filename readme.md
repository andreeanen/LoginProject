Din uppgift:
1. Skapa ett C# MVC projekt, bygg en simpel Anv/Lösenord sida med en logga in knapp.
2. När du trycker på Logga in, ska du skicka ditt anv/lösenord till din controller som i sin tur ska göra ett REST anrop mot vår service.

Vår service bryr sig inte om vad du anger för användarnamn och lösenord, huvudsaken är att anropet är korrekt utformat.

Type: POST
URL: https://services2.i-centrum.se/recruitment/auth
Body json:
{
   "username": "From_input",
   "password": "From_Input"
}

3. Är ditt anrop korrekt kommer du få tillbaka en Access Token du kan använda för att autentisering.
4. Gör följande anrop:

Type: GET
URL: https://services2.i-centrum.se/recruitment/profile/avatar
Skicka med din token som bearer i headern.

5. Om autentiseringen lyckades så kommer du få en json tillbaka innehållande data.

Denna data är en base64 av en bild, visa upp denna bild i ditt front-end, antingen direkt på inloggningssidan eller en ny sida.

Nästa steg i processen står i bilden.
