package Sender;


import Model.Packet;

import java.util.concurrent.ThreadLocalRandom;

public class Sender implements Runnable {
    private Packet pack;

    public Sender(Packet pack) {
        this.pack = pack;
    }

    @Override
    public void run() {
        int[] packeges = {1, 0, 0, 1, 0, 1, 0, 0, 5};

        for (int packs : packeges) {
            if (packs == 1) {
                System.out.println("Sender " + packs);
            } else {
                pack.send(packs);
            }
            try {
                Thread.sleep(ThreadLocalRandom.current().nextInt(1000, 5000));
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
            }
        }
    }
}
