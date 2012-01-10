

My ongoing work to create a full feature ASP.NET MVC framework the JQ Grid. When I started looking at the grid, I could not get all the information for hooking up the grid to the MVC framework in one place. This is my attempt to start and bring the info together -- hope the community will improve it. Also wanted to create a sort of a "started project" which used Massive and the Blueprint CSS System.

Features
-----------
1.Uses MVC3 Razor

2.Uses the Open Source JQ Grid. http://www.trirand.com/jqgridwiki/doku.php

3.Uses Massive for database interaction.https://github.com/robconery/massive

4.Has built in Search, CRUD, Comboboxes ... with REST.

5.Uses JQuery UI for theming. Includes multiple themes: dot-luv,humanity,redmond,start,ui-darkness,ui-lightness,blitzer,dark-hive,eggplant,le-frog,mint-choc,south-street,trontastic. http://jqueryui.com/

6.Hooked up to the Blueprint CSS System. http://blueprintcss.org/tests/parts/sample.html


7.Highly influenced by the many creative ideas of Rob Conery , the creator of Massive, especially the ideas presented in his screencasts Real World MVC. Do invest $32 and watch the series. It will do you good.http://tekpub.com/productions/mvc3


8. Security: add attribute [RequireSecurity] on a controller method to enable security. Uses a local table (users) and bypasses the ASP.NET membership database. Look at these files under 	Intrastructure folder: FormsAuthTokenStore, RequireSecurityAttribute. Ninject injects FormsAuthTokenStore--> look at App_Start/NinjectMVC3.cs/ RegisterServices method.

This is my attempt to give something back to the developer community for all the help I have received… everything is freely available under the BSD source license, which is documented in the read me. Please let me know if you find a use for framework in your app.

Fork it and improve so that the community has a full featured MVC JGrid in one place.

Usage
-------

1. backup of sample database is in \sql folder. Restore it in Sql Server

2. To change theme: In shared\_layout.cshtml change the tag : @Assets.jgrid("start")  to any of these:dot-luv, humanity, redmond,start,ui-darkness,ui-lightness,blitzer,  dark-hive,eggplant,le-frog, mint-choc,south-street,trontastic

               
Harsh Bhasin
http://harshbhasin.com
