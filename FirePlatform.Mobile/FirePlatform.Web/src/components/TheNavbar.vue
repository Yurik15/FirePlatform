<template>
<div>
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
        <vue-progress-bar></vue-progress-bar>
        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNavDropdown" aria-controls="navbarNavDropdown" aria-expanded="false" aria-label="Toggle navigation">
    <span class="navbar-toggler-icon"></span>
  </button>
        <div class="collapse navbar-collapse" id="navbarNavDropdown">
            <ul class="navbar-nav">
                <li class="nav-item dropdown">
                    <a class="nav-link dropdown-toggle navibarText" href="#" id="navbarDropdownMenuLink" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
          Templates
        </a>
                    <ul class="dropdown-menu" aria-labelledby="navbarDropdownMenuLink">
                        <li no-body v-for="(template, index) in templates" v-bind:value="template.id" :key="index">
                            <button class="menuButton" type="button" v-on:click="getTemplate(template.id)">{{template.name}}</button>
                        </li>
                    </ul>
                </li>
            </ul>
        </div>
    </nav>
    </div>
</template>

<script>
import axios from 'axios'
import templatesService from '@/api-services/templates.service';
import {
    serverBus
} from '../main.js';

export default {
    
    name: 'TheNavbar',
    data: () => ({
        templates: [],
        loading: false
    }),
    methods: {
        getTemplate: function (templateId) {
            this.$Progress.start()
            templatesService.loadTemplatesData(templateId)
                .then(response => {
                    this.$Progress.finish();
                    serverBus.$emit('itemsGroup', response.data);
                })
                .catch(e => {
                    this.errors.push(e)
                })
        }
    },
    created() {
        this.$Progress.start()
        templatesService.getAll()
            .then(response => { 
                this.$Progress.finish();
                this.loading = true;

                this.templates = response.data;
            })
            .catch(e => {
                this.errors.push(e)
            })
    }
}
</script>

<style>
.menuButton {
    color: #e28822;
    background-color: #343a40;
    width: 100%
}
</style>
