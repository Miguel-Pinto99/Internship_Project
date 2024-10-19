using MQTTnet.Client;
using MQTTnet;
using Project1.Infrastructure;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using BlazorApp1.Model;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.Extensions.FileSystemGlobbing;
using System.Threading;
using Microsoft.AspNetCore.CookiePolicy;

namespace BlazorApp1.Infrastructure
{
    public class UnsService : IDisposable
    {
        private IMqttClient? _mqttClient;
        public List<ApplicationUser> listApplicationUsers = new List<ApplicationUser>();

        public void Dispose()
        {
           _mqttClient?.Dispose();
           _mqttClient = null;
        }

        public async Task<List<ApplicationUser>> SubscribeBrokerAsync(CancellationToken cancellationToken)
        {
            if(_mqttClient is null)
            { 
                var mqttFactory = new MqttFactory();
                _mqttClient = mqttFactory.CreateMqttClient();
                var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("127.0.0.1").Build();
                await _mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(
                        f =>
                        {
                            f.WithTopic("users/#");
                        })
                    .Build();

                await _mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);



                _mqttClient.ApplicationMessageReceivedAsync += async message =>
                {
                    string messagePayLoad = System.Text.Encoding.UTF8.GetString(message.ApplicationMessage.Payload);
                    string messageTopic = message.ApplicationMessage.Topic;

                    List<string> listPattern = new List<string>();
                    listPattern.Add(@"\b(Schedule)\b$");
                    listPattern.Add(@"\b(today)\b$");

                    ApplicationUser applicationUser = new ApplicationUser();

                    string patternId = @"[1-9]+";
                    string patternSchedule = @"\b(Schedule)\b$";
                    string patternToday = @"\b(today)\b$";
                    string patternCheckedIn = @"\b(checked_in)\b$";
                    string patternTrueFalse = @"(true)|(false)";


                    Match matchId = Regex.Match(messageTopic, patternId);
                    bool matchWorkToday = Regex.IsMatch(messageTopic, patternSchedule);
                    bool matchTodayShift = Regex.IsMatch(messageTopic, patternToday);
                    bool matchCheckedIn = Regex.IsMatch(messageTopic, patternCheckedIn);

                    int id = Convert.ToInt32(matchId.Value);
                    var userInList = listApplicationUsers.Find(x => x.Id == id);
                    applicationUser.Id = id;

                    if (userInList != null)
                    {
                        applicationUser.ScheduleWorkToday = userInList.ScheduleWorkToday;
                        applicationUser.TodayShift = userInList.TodayShift;
                        applicationUser.Checked_In = userInList.Checked_In;
                        listApplicationUsers.RemoveAll(x => x.Id == id);
                    }

                    if (matchWorkToday)
                    {
                        Match matchTrueFalse = Regex.Match(messagePayLoad, patternTrueFalse);
                        string scheduleWorkTodayString = Convert.ToString(matchTrueFalse);
                        bool scheduleWorkToday = Convert.ToBoolean(scheduleWorkTodayString);
                        applicationUser.ScheduleWorkToday = scheduleWorkToday;
                    }

                    if (matchTodayShift)
                    {
                        string TodayShift = Convert.ToString(messagePayLoad);
                        applicationUser.TodayShift = TodayShift;
                    }

                    if (matchCheckedIn)
                    {
                        Match matchTrueFalse = Regex.Match(messagePayLoad, patternTrueFalse);
                        string checkedInString = Convert.ToString(matchTrueFalse);
                        bool checkedIn = Convert.ToBoolean(checkedInString);
                        applicationUser.Checked_In = checkedIn;
                    }

                    listApplicationUsers.Add(applicationUser);
                };
            }
            return listApplicationUsers;
        }
        


    }
}
