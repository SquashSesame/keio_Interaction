import os
from flask import Flask, request
from dbConfig import DB
from dbModel import PlayerPos

app = Flask(__name__)
app.secret_key = "admin"

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



"""
    データベースに登録する
"""
@app.route("/set_pos", methods=["POST"])
def post_setpos():
    data = request.get_json()
    # データを受け取って、データモデルへ登録
    item = PlayerPos()
    item.message = data['message']
    item.name = data['strName']
    item.px = data['px']
    item.py = data['py']
    item.pz = data['pz']
    # テーブル追加登録
    DB.add(item)
    DB.commit()
    return "OK"


if __name__ == '__main__':
    app.debug = True
    app.secret_key = os.urandom(12)
    print("Launch server")
    app.run(host='0.0.0.0', port=880)
