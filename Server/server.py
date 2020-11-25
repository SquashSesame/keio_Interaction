from flask import Flask, request

app = Flask(__name__)

@app.route("/")
def hello_world():
    return 'Ready Server!'

@app.route("/log", methods=["POST"])
def post_log():
    data = request.form['data']
    print("POST : " + data)
    return "OK"


@app.route("/log", methods=["GET"])
def get_log():
    data = request.args.get("data")
    print("GET : " + data)
    return "OK"


if __name__ == '__main__':
    app.debug = True
    app.run(host='0.0.0.0', port=880)
