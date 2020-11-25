from flask import Flask, request

app = Flask(__name__)

@app.route("/")
def hello_world():
    return 'Ready Server!'

@app.route("/log", methods=["POST"])
def log_post():
    data = request.form['area']
    return  "POST :" + data

@app.route("/log", methods=["GET"])
def log_get():
    area = request.args.get("area")
    return "GET : " + area

if __name__ == '__main__':
    app.debug = True
    app.run(host='0.0.0.0', port=880)
