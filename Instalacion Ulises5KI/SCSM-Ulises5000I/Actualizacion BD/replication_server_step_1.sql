create user 'repl'@'%' identified by 'slavepass';
grant replication slave on *.* to 'repl'@'%' identified by 'slavepass';
