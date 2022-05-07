package Receiver;

import Model.Packet;

import java.util.Objects;

public class Receiver implements Runnable {
    private final String name = "Sonny";
    private Packet loadPacket;

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
