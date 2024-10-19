# Hoppenbrouwers project from Miguel Pinto 2023

## Description

Repository which contains my internship project at Hoppenbrouwers. The premise of this project was to connect a front-end application with a back-end application through an MQTT broker. The idea was to build a fully functional dashboard which could be shown at the Hoppenbrouwers factory floors in every location across the Netherlands. To keep the dashboard updated, the software subscribes to different topics on an MQTT-Broker which is being updated by the back-end application. The back-end application is a REST API coded in c#, placed in a folder named "miguel-pvs". The API uses a local database to store the data regarding personal information about each employee (name, id, location). The application knows each employee state by overlaying his work pattern with the check-in state, the absent registers and the current time. The API is also equipped with an MQTT publisher, which updates the broker when a client tries to alter any kind of data in the local database.

## Context
The dashboard shows which employees are currently working, absent and missing. Let's assume worker A needed help building an electric board because he was behind schedule. By checking the dashboard, he would know right away that calling employee B would be pointless because he reported sick and is absent. Conversely, he sees that employee C has already checked in with his company card, so he might be available to give him a hand.

## Used tools/Frameworks:

```
-REST API
-MQTT
-Unitests
-mediaR notifications
-SQL
```

## Deployment

To run the application, the following tools/libraries are needed:

```
1- Swagger (It allows the user to simulate the check-in and it should launch automatically with the application)
2- MQTT broker (setup a container with EMQX for example)
```

## Launch

To start the project, first, a docker container with an EMQX MQTT broker should be started. Then start the front-end and back-end and let the application do its magic. 
