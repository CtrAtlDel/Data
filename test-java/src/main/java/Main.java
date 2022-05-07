public class Main {
    public static void main(String[] args) {
        Thread t = new Thread("qwert"); // получаем главный поток
        System.out.println(t.getName()); // main
    }
}
