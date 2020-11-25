from sqlalchemy import Column, Integer, String, Float, DateTime, Boolean
from dbConfig import Base

#db = SQLAlchemy(app)

# データモデル
class PlayerPos(Base):
   __tablename__ = 'history'
   id = Column('id', Integer, primary_key = True, autoincrement=True)
   message = Column('message', String(100))
   name = Column('name', String(100))
   px = Column('px', Float)
   py = Column('py', Float)
   pz = Column('pz', Float)
