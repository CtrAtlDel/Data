public class Package {

    private boolean transfer = true;
    private int iterator = 0;
    private final int[] bitArray = {0, 1, 0, 0, 1, 0, 1, 1};

    public synchronized void send(int iterator) {  // Отправка пакета с итератором
        while (!transfer) {
            try {
                wait();
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
                System.out.println("Thread Interrupted");
            }
        }

        this.transfer = false;
        this.iterator = iterator;
        notifyAll();
    }

    //Получение пакета с итератором
    public synchronized int receive() {

        while (transfer) {
            try {
                wait();
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
                System.out.println("Thread Interrupted");
            }
        }

        transfer = true;
        int result = this.iterator;
        notifyAll();
        return result;
    }

}
