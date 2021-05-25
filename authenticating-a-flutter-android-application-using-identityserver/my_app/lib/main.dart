import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:json_annotation/json_annotation.dart';
import 'package:oauth2/oauth2.dart' as oauth2;
import 'package:uni_links/uni_links.dart';
import 'package:url_launcher/url_launcher.dart';

part 'main.g.dart';

final authorizationEndpoint = Uri.parse(
  'https://xyz.ngrok.io/connect/authorize',
);
final tokenEndpoint = Uri.parse(
  'https://xyz.ngrok.io/connect/token',
);

final identifier = 'flutter';

final redirectUrl = Uri.parse(
  'com.example.myapp://callback',
);

Future<oauth2.Client> createClient() async {
  // If we don't have OAuth2 credentials yet, we need to get the resource owner
  // to authorize us. We're assuming here that we're a command-line application.
  var grant = oauth2.AuthorizationCodeGrant(
    identifier,
    authorizationEndpoint,
    tokenEndpoint,
  );

  // A URL on the authorization server (authorizationEndpoint with some additional
  // query parameters). Scopes and state can optionally be passed into this method.
  var authorizationUrl = grant.getAuthorizationUrl(
    redirectUrl,
    scopes: [
      'openid',
      'profile',
      'offline_access',
      'api',
    ],
  );

  // Redirect the resource owner to the authorization URL. Once the resource
  // owner has authorized, they'll be redirected to `redirectUrl` with an
  // authorization code. The `redirect` should cause the browser to redirect to
  // another URL which should also have a listener.
  //
  // `redirect` and `listen` are not shown implemented here. See below for the
  // details.
  await redirect(authorizationUrl);
  var responseUrl = await listen(redirectUrl);

  if (responseUrl == null) {
    throw Exception('Response URL was null.');
  }

  // Once the user is redirected to `redirectUrl`, pass the query parameters to
  // the AuthorizationCodeGrant. It will validate them and extract the
  // authorization code to create a new Client.
  return await grant.handleAuthorizationResponse(responseUrl.queryParameters);
}

Future<void> redirect(Uri authorizationUrl) async {
  if (await canLaunch(authorizationUrl.toString())) {
    await launch(authorizationUrl.toString());
  } else {
    throw Exception('Unable to launch authorization URL.');
  }
}

Future<Uri?> listen(Uri redirectUrl) async {
  return await uriLinkStream.firstWhere(
    (element) => element.toString().startsWith(
          redirectUrl.toString(),
        ),
  );
}

Future<List<WeatherForecast>> getWeatherForecasts(oauth2.Client client) async {
  final response = await client.get(Uri.parse('/WeatherForecast'));
  return (json.decode(response.body) as List)
      .map((e) => WeatherForecast.fromJson(e))
      .toList();
}

void main() {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      home: MyHomePage(
        title: 'Flutter Demo Home Page',
      ),
    );
  }
}

class MyHomePage extends StatefulWidget {
  MyHomePage({Key? key, required this.title}) : super(key: key);

  final String title;

  @override
  _MyHomePageState createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  late Future<List<WeatherForecast>> _future;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(widget.title),
      ),
      body: FutureBuilder<List<WeatherForecast>>(
        future: _future,
        builder: (context, snapshot) => snapshot.hasData
            ? ListView.builder(
                itemBuilder: (context, index) => ListTile(
                  title: Text(snapshot.data![index].summary ?? ''),
                ),
                itemCount: snapshot.data!.length,
              )
            : Center(
                child: CircularProgressIndicator(),
              ),
      ),
    );
  }

  @override
  void initState() {
    super.initState();
    _future = createClient().then((value) => getWeatherForecasts(value));
  }
}

@JsonSerializable()
class WeatherForecast {
  String? summary;

  WeatherForecast({
    this.summary,
  });

  factory WeatherForecast.fromJson(Map<String, dynamic> json) {
    return _$WeatherForecastFromJson(json);
  }

  Map<String, dynamic> toJson() {
    return _$WeatherForecastToJson(this);
  }
}
