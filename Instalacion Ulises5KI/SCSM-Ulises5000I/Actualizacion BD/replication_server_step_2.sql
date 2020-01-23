set @ip='192.168.0.111';

stop slave;
reset master;
show master status;

reset slave;

set @s=concat("change master to master_host='",@ip,"',master_user='repl',master_password='slavepass',master_log_file='mysql-bin.000001',master_log_pos=120");

prepare st from @s;
execute st;
deallocate prepare st;

show master status;
