package Sender;

import IGotYouBabe.Song;
import Model.Packet;

import java.util.Objects;

public class Sender implements Runnable {

    private final String name = "Cher";
    private Packet pack;


    public Sender(Packet pack) {
        this.pack = pack;
    }

    @Override
    public void run() {
        Song song = new Song();
        String[][] end = {{"."}, {"."}};
        String[][] lyrics = song.lyrics;

        for (String[] lyric : lyrics) {
            if (Objects.equals(lyric[0], "Cher")) {
                System.out.println(this.name + ": " + lyric[1]);
            } else if (Objects.equals(lyric[0], "Sonny")) {
                pack.send(lyric[1]);
            } else if (lyric[0] == "Sonny, Cher") {
                System.out.println(this.name + ": " + lyric[1]);
                pack.send(lyric[1]);
            }
        }

//        for (String packages : lyrics) {
//            if (packages == 1) {
//                System.out.println(this.name + packages);
//            } else {
//                if (packages == 3) {
//                    System.out.println(this.name + packages);
//                    pack.send(packages);
//                } else {
//                    pack.send(packages);
//                }
//            }
//            try {
//                Thread.sleep(ThreadLocalRandom.current().nextInt(1000, 5000));
//            } catch (InterruptedException e) {
//                Thread.currentThread().interrupt();
//            }
//        }
    }
}
