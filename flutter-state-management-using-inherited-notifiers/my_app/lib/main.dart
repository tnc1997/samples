import 'package:flutter/material.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatefulWidget {
  const MyApp({Key? key}) : super(key: key);

  @override
  State<MyApp> createState() => _MyAppState();
}

class _MyAppState extends State<MyApp> {
  final _counterStateData = CounterStateData();

  @override
  Widget build(BuildContext context) {
    return CounterState(
      notifier: _counterStateData,
      child: const MaterialApp(
        title: 'Flutter Demo',
        home: MyHomePage(),
      ),
    );
  }

  @override
  void dispose() {
    _counterStateData.dispose();
    super.dispose();
  }
}

class MyHomePage extends StatelessWidget {
  const MyHomePage({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Flutter Demo Home Page'),
      ),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            const Text(
              'You have pushed the button this many times:',
            ),
            Text(
              '${CounterState.of(context).counter}',
              style: Theme.of(context).textTheme.headline4,
            ),
          ],
        ),
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: CounterState.of(context).increment,
        tooltip: 'Increment',
        child: const Icon(Icons.add),
      ),
    );
  }
}

class CounterState extends InheritedNotifier<CounterStateData> {
  const CounterState({
    Key? key,
    required CounterStateData notifier,
    required Widget child,
  }) : super(
          key: key,
          notifier: notifier,
          child: child,
        );

  static CounterStateData of(BuildContext context) {
    return context
        .dependOnInheritedWidgetOfExactType<CounterState>()!
        .notifier!;
  }
}

class CounterStateData extends ChangeNotifier {
  int _counter = 0;

  int get counter => _counter;

  set counter(int value) {
    if (_counter != value) {
      _counter = value;
      notifyListeners();
    }
  }

  void increment() {
    _counter++;
    notifyListeners();
  }
}
