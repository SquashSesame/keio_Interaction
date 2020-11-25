from sqlalchemy import create_engine
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import scoped_session, sessionmaker
import os


# sqlite
basedir = os.path.abspath(os.path.dirname(__file__))
SQLALCHEMY_DATABASE_URI = 'sqlite:///' + os.path.join(basedir, './database.sqlite3')

"""
# mysql
SQLALCHEMY_DATABASE_URI = 'mysql+pymysql://{user}:{password}@{host}/flask_sample?charset=utf8'.format(**{
    'user': os.getenv('DB_USER', 'root'),
    'password': os.getenv('DB_PASSWORD', ''),
    'host': os.getenv('DB_HOST', 'localhost'),
})
"""



SQLALCHEMY_TRACK_MODIFICATIONS = False
SQLALCHEMY_ECHO = False

ENGINE = create_engine(SQLALCHEMY_DATABASE_URI, encoding="utf-8", echo=True)

DB = scoped_session(
    sessionmaker(
        bind=ENGINE
    )
)

Base = declarative_base()
Base.query = DB.query_property()
