public class CustomThread implements Runnable {
    String person; // name of thread

    public CustomThread(String person) {
        this.person = person;
    }

    public void run() {

    }

    public String getPerson() {
        return person;
    }
}

