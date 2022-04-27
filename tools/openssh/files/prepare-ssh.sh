#!/bin/bash

passwd root <<EOF
P@ssw0rd
P@ssw0rd
EOF

# Generate unique ssh keys for this container, if needed
if [ ! -f /etc/ssh/id_key ]; then
    ssh-keygen -t ed25519 -f /etc/ssh/id_key -N ''
fi

# Restrict access from other users
chmod 600 /etc/ssh/id_key

mkdir -p /root/.ssh
cat /tmp/sftp-key.pub >> /root/.ssh/authorized_keys
chmod -R 700 /root/.ssh && chmod -R 600 /root/.ssh/*
