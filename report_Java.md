## Решения

### Eats

> Условие: 
> 30_000_000 записей всего
> 18_758_328 уникальных номеров телефонов
> ~ 20 длина полного имени

Пусть оперативная память ничем не ограничена. (В условии не указано). Для примеров будем использовать Java 8.

#### Решение задачи 1)

Чтобы поиск выполнялся со сложностью в среднем ~ О(1) необходимо использовать структуру данных - хеш-таблица (метод перемешивания или сцепления). Желательно будет оптимизировать хеш - функцию, чтобы она потребляла меньше ресурсов и возникало как можно меньше коллизий и сохранять ее значения вместе с ключом + не будем сильно задумывать можно ли будет. 

Будем хранить все записи в таблице и использовать в качестве ключа - значение хеш функции, а в качестве значения - запись о пользователе. 

Не будем хранить повторные записи (запись будет создавать коллизию поскольку будет браться хеш от такого же номера телефона).

В Java хеш таблица представлена как HashMap.

Тогда нам потребуется вот столько памяти на полную запись:
1. В одной элементе будем использовать две строки типа (String). Заголовок в String занимает - 8 байт
2. Длина массива (int) -  4 байта
3. Ссылка на char[] (1 символ занимает 2 байта), а у нас в среднем 20 символов - 40 байт
4. Сслыка на номер телефона (его тоже будем держать в памяти) ~ 10 символов по 2 байта = 20 байт
5. Суммарно на одно полное имя - 8 + 4 + 40 + 20 = 72 байта

Хеш можно хранить как int и одни хеш будет занимать 4 байта

Суммарно одна запись будет занимать 56 байт, таких записей будет 18_758_328 * 72 = 1_350_599_616 байт памяти для хранения нашей базы.

Ps Плюс еще различный системные элементы, которые требует сам язык/платформа и его структуры.

--- 

#### Решение задачи 2)

Мысль №1 - если хотим сэкономить на памяти, должны быть тяжелые вычисления или вероятностные алгоритмы.

Стоит отметитить что в условии нет информации о том, нужно ли хранить упорядоченную структуру. Будем считать что неупорядоченную.

Лобовое решение: можно взять линейный список (LinkedList), который будет хранить только ссылку на следующий элемент и массив char(2 * 20 = 40 байт) - запись пользователя и 10 символов 20 байт размер номера телефона. Это будет стоить 18_758_328 * 60 = 1_125_499_680

Ps Плюс еще различный системные элементы которые требует сам язык/платформа и его структуры (в них входит ссылка на объект)

---

### I got you

Описание решения: Будем использовать принцип Producer - Consumer. Возможно можно было решить данную задачу с помощью блокировок, но мое решение более общее и может быть применимо к разного рода схожим задачам. Основной принцип работы будет вот такой:

> Первый поток "Cher" будет итеративно идти по именам и выводить строчки, если его имя совпадает с "Cher", то печатаем строчки текста. Если первый поток нашел "Sonny", то передается информация во второй поток и выводится с помощью него. Если встретилось "Sonny, Cher", то будем выводить сначала первым потоком, а потом выводить его во втором.

Сделаем класс с песней (чтобы точно был ООП стиль):

```Java
package IGotYouBabe;


public final class Song {
    public final
    String[][] lyrics = {
            {"Cher", "They say we're young and we don't know \nWe won't find out until we grow"},
            {"Sonny", "Well I don't know if all that's true \n'Cause you got me, and baby I got you"},
            {"Sonny", "Babe"},
            {"Sonny, Cher", "I got you babe \nI got you babe"},
            {"Cher", "They say our love won't pay the rent \nBefore it's earned, our money's all been spent"},
            {"Sonny", "I guess that's so, we don't have a pot \nBut at least I'm sure of all the things we got"},
            {"Sonny", "Babe"},
            {"Sonny, Cher", "I got you babe \nI got you babe"},
            {"Sonny", "I got flowers in the spring \nI got you to wear my ring"},
            {"Cher", "And when I'm sad, you're a clown \nAnd if I get scared, you're always around"},
            {"Cher", "So let them say your hair's too long \n'Cause I don't care, with you I can't go wrong"},
            {"Sonny", "Then put your little hand in mine \nThere ain't no hill or mountain we can't climb"},
            {"Sonny", "Babe"},
            {"Sonny, Cher", "I got you babe \nI got you babe"},
            {"Sonny", "I got you to hold my hand"},
            {"Cher", "I got you to understand"},
            {"Sonny", "I got you to walk with me"},
            {"Cher", "I got you to talk with me"},
            {"Sonny", "I got you to kiss goodnight"},
            {"Cher", "I got you to hold me tight"},
            {"Sonny", "I got you, I won't let go"},
            {"Cher", "I got you to love me so"},
            {"Sonny, Cher", "I got you babe \nI got you babe \nI got you babe \nI got you babe \nI got you babe"}
    };
}
```
 В роле Prodecer - Sender, вот его описани:

```Java
package Sender;

import IGotYouBabe.Song;
import Model.Packet;

import java.util.Objects;

public class Sender implements Runnable {
    private final String name = "Cher";
    private final Packet pack;

    public Sender(Packet pack) {
        this.pack = pack;
    }

    @Override
    public void run() {
        Song song = new Song();
        String[][] lyrics = song.lyrics;

        for (String[] lyric : lyrics) {
            if (Objects.equals(lyric[0], "Cher")) {
                System.out.println(this.name + ": " + lyric[1]);
            }
            else if (Objects.equals(lyric[0], "Sonny")) {
                pack.send(lyric[1]);
            }
            else if (Objects.equals(lyric[0], "Sonny, Cher")) {
                System.out.println(this.name + ": " + lyric[1]);
                pack.send(lyric[1]);
            }

            try {
                Thread.sleep(1000);
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
            }

        }
    }
}
```

В роли Consumer - Receiver, он будет получаеть информацию и выводить ее.

```Java
package Receiver;

import Model.Packet;

import java.util.Objects;

public class Receiver implements Runnable {
    private final String name = "Sonny";
    private final Packet loadPacket;

    public Receiver(Packet loadPacket) {
        this.loadPacket = loadPacket;
    }

    @Override
    public void run() {
        for (String receivedMessage = loadPacket.receive(); !Objects.equals(receivedMessage, ".");
             receivedMessage = loadPacket.receive()) {

            System.out.println(this.name + ": " + receivedMessage);
            try {
                Thread.sleep(1000);
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
            }

        }
    }
}
```

Общаться треды будут с помощью класса Packet. В этот класс будет записывать информацию поток 1 и ждать пока поток 2 ее прочитает. Для этого добавлены  Thread.sleep(1000).

```Java
package Model;


public class Packet {

    private boolean transfer = true;
    private String phrase;


    public synchronized void send(String data) {   
        while (!transfer) {
            try {
                wait();
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
                System.out.println("Thread Interrupted");
            }
        }
        this.transfer = false;

        this.phrase = data;

        notifyAll();
    }

    public synchronized String receive() {

        while (transfer) {
            try {
                wait();
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
                System.out.println("Thread Interrupted");
            }
        }

        transfer = true;
        String result = this.phrase;
        notifyAll();
        return result;
    }

}
```


