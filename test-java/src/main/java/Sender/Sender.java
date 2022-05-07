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
