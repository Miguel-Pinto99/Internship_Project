# HoppenbrouwersProject

This is the repository of my internship project at Hoppenbrouwers. The premise of this project was to connect a front-end application with a back-end application through an MQTT broker. The ideia was to build a fully functional dashboard which could be shown at the Hoppenbrouwers factory floors in every location across the Netherlands. The dashboard shows which employees are currently working, absent and missing. ts assume a worker A needed help building a eletric board because he was behind schedule. By checking the dashboard, he would know right away that calling employee B would be pointless because he reported sick and therefore he is absent.
The backend application is a REST API coded in c# named Miguel-PVS. It updates the information shown at the dashboard by accessing a local database. 
Designed a fully functional application, with a REST API coded in c# as the back-end and a Razor Application built upon HTML as the front-end.aa
To start the project first a dockainer container with an EMQX MQTT broker should be started. Then start the front-end and back-end applications do their magic 
