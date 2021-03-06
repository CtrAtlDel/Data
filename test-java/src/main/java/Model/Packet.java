package Model;


public class Packet {

    private boolean transfer = true;
    private String phrase;


    public synchronized void send(String data) {  // Отправка пакета с итератором
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
