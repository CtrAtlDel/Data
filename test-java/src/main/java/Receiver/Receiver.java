package Receiver;

import Model.Packet;

import java.util.concurrent.ThreadLocalRandom;

public class Receiver implements Runnable {
    private Packet loadPacket;

    public Receiver(Packet loadPacket) {
        this.loadPacket = loadPacket;
    }

    @Override
    public void run() {
        for (int receivedMessage = loadPacket.receive(); receivedMessage != 5;
             receivedMessage = loadPacket.receive()) {
            System.out.println("Receiver " + receivedMessage);

            try {
                Thread.sleep(ThreadLocalRandom.current().nextInt(1000, 5000));
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
            }

        }
    }
}
