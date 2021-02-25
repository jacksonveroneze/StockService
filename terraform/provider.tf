terraform {
  required_providers {
    digitalocean = {
      source = "digitalocean/digitalocean"
      version = "1.22.2"
    }
  }
}

variable "do_token" {}
variable "pvt_key" {}

provider "digitalocean" {
  token = var.do_token
}

data "digitalocean_ssh_key" "terraform" {
  name = "Default"
}

terraform apply -var "do_token=$DO_TOKEN" -var "pvt_key=$HOME/.ssh/id_rsa"
terraform plan -out=infra.out -var "do_token=$DO_TOKEN" -var "pvt_key=$HOME/.ssh/id_rsa"
