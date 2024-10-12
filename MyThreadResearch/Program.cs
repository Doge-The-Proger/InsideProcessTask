/*Напишите вычисление суммы элементов массива интов:
1. Обычное
2. Параллельное (для реализации использовать Thread, например List) - взять 2/4/6/8 потоков, в рамках которых обрабатываются соответствующие блоки чисел из нужных блоков
3. Параллельное с помощью LINQ
Замерьте время выполнения для 100 000, 1 000 000 и 10 000 000*/

const int MIN_COUNT_OF_VALUE = 0;
//const int ELEMENT_COUNT = 100_000;
//const int ELEMENT_COUNT = 1_000_000;
const int ELEMENT_COUNT = 10_000_000;

const int TASK_COUNT = 5;

Console.WriteLine($"Привет!\n\nДалее будут запущены разные подходы к подсчёту суммы элементов в массиве\nВ исходном массиве - {ELEMENT_COUNT} елементов\n");


var min = 0;
var max = 10;
var random = new Random();

// 1. Создание и заполнение массива случайных чисел
var randArray = Enumerable.Repeat(MIN_COUNT_OF_VALUE, ELEMENT_COUNT).Select(i => random.Next(min, max)).ToArray();

// 2. Подсчёт суммы элементов

// 2.1 Классическое суммирование
var now = DateTime.Now;
var commonSum = randArray.Sum();
Console.WriteLine($"Сумма элементов, полученные вызовом Sum - {commonSum}\nВремя выполнения - {DateTime.Now - now}\n\n");

// 2.2 Суммирование в потоках
now = DateTime.Now;
var taskList = new Task<long>[TASK_COUNT];
var numBlockSize = randArray.Length / TASK_COUNT;

for (int i = 0; i < TASK_COUNT; i++)
{
    //Получение части массива, перед передачей в поток
    var nums = randArray.Skip(i * numBlockSize).Take(numBlockSize);

    taskList[i] = new Task<long>(() => GetSum(nums, i + 1));
    taskList[i].Start();
};

Task.WaitAll(taskList);

var threadSum = taskList.Select(t => t.Result).Sum();
Console.WriteLine($"\nСумма элементов, полученные путём применения {TASK_COUNT} потоков для суммирования - {threadSum}\n\n");

// 2.3 Суммирование при помощи PLINQ

now = DateTime.Now;
var parallelSum = randArray.AsParallel().Sum(); ;

Console.WriteLine($"\nСумма элементов, полученные путём применения PLINQ - {parallelSum}\nВремя выполнения - {DateTime.Now - now}\n\n");

Console.WriteLine("The End");


long GetSum(IEnumerable<int> nums, int index)
{
    var sum = nums.Sum();
    Console.WriteLine($"Поток {index} завершил вычисление.\nВремя выполнения потока итоговое - {DateTime.Now - now}");
    return nums.Sum();
}


