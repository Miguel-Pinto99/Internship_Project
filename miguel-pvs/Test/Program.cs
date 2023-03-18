// See https://aka.ms/new-console-template for more information
using System.Diagnostics;





//INICIALIZAÇAO DO TIMER E COMAND LINE
Console.WriteLine("Hello, World!");
var stopwatch = new Stopwatch();

stopwatch.Start();

// COMO ISTO NAO ESTA AWAIT, SALTA TUDO ATE AO IMPRIMIR 
var task1 = Task.Delay(TimeSpan.FromSeconds(2));
var task2 = Task.Delay(TimeSpan.FromSeconds(3));
var task3 = Task.Delay(TimeSpan.FromSeconds(5));
Console.WriteLine($"Combined task is finished in {stopwatch.Elapsed} but should be 10 with await");

// ISTO ESTA AWAIT LOGO ESPERA UM BECKS ATE IMPRIMIR PROXIMA MENSAGEM
var taskList = new List<Task> {task1,task2,task3 };
var combTask = Task.WhenAll(taskList);
await combTask;

Console.WriteLine($"Combined task is finished in {stopwatch.Elapsed} because it awaited every task at once");


// AWAIT UM BECKS PARA MOSTRAR PROXIMA TRENCH DE CODIGO

await Task.Delay(TimeSpan.FromSeconds(5));

//INICIALIZAÇAO DO TIMER E COMAND LINE
Console.WriteLine("VAMOS LA COMECAR DE NOVO ESPERA 5 SEGUNDOS");

await Task.Delay(TimeSpan.FromSeconds(5));

// ESPERA 2 SEGUNDOS
Console.WriteLine("PRIMEIRA TASK: ESPERA 2S");
task1 = Task.Delay(TimeSpan.FromSeconds(2));
await task1;

task2 = Task.Delay(TimeSpan.FromSeconds(3));
task3 = Task.Delay(TimeSpan.FromSeconds(5));
Console.WriteLine("CALMAAAAAAAAAAA");
// ESPERA 5 SEGUNDOS PORQUE DUAS TAREFAS ESTAO A CORRER EM SIMULTANEO E UMA JA FOI
taskList = new List<Task> { task1, task2, task3 };
await Task.WhenAll(taskList);


//NO FINAL TEM DE DAR 22S
Console.WriteLine($"Combined task is finished in {stopwatch.Elapsed}");

