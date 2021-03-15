describe('todos API', () => {
    it.only('returns JSON', () => {
        cy.request({
            url: 'http://127.0.0.1:5000/api/v1/products?description=a&take=6&skip=1&isActive=true',
            headers: {
                'authorization': 'Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IlJETkVNalV5UXpGRE56ZENOVEpGUXpoQ1JFSXpNMFl6T0VJd016TXdNRUpFTWtaRk5qVTBSZyJ9.eyJpc3MiOiJodHRwczovL2phY2tzb252ZXJvbmV6ZS5hdXRoMC5jb20vIiwic3ViIjoiZEhCbkl1ZFV1bkt3OHBZUVBjZGpKcjQwQU9KeVUxbHRAY2xpZW50cyIsImF1ZCI6Imh0dHBzOi8vc3RvY2stamFja3NvbnZlcm9uZXplLmF6dXJld2Vic2l0ZXMubmV0IiwiaWF0IjoxNjE1NTgyMDYzLCJleHAiOjE2MTU2Njg0NjMsImF6cCI6ImRIQm5JdWRVdW5LdzhwWVFQY2RqSnI0MEFPSnlVMWx0Iiwic2NvcGUiOiJwcm9kdWN0czpmaWx0ZXIgcHJvZHVjdHM6ZmluZCBwcm9kdWN0czpjcmVhdGUgcHJvZHVjdHM6dXBkYXRlIHByb2R1Y3RzOmRlbGV0ZSBwdXJjaGFzZXM6ZmlsdGVyIHB1cmNoYXNlczpmaW5kIHB1cmNoYXNlczpjcmVhdGUgcHVyY2hhc2VzOmNsb3NlIHB1cmNoYXNlczpmaW5kLWl0ZW1zIHB1cmNoYXNlczp1cGRhdGUtaXRlbSBwdXJjaGFzZXM6cmVtb3ZlLWl0ZW0gcHVyY2hhc2VzOmNyZWF0ZS1pdGVtIHB1cmNoYXNlczpmaW5kLWl0ZW0gcHVyY2hhc2VzOmRlbGV0ZSIsImd0eSI6ImNsaWVudC1jcmVkZW50aWFscyJ9.SJFtKyAev8Ww16ctUpQgYLrMdBDk8bAjtbfa4vF2dYBaWTGbjiPK9O7k6Q2iwwU7FVw8W0ylvhNWvYmYwUX52d0g_PcQTjk6-NZhP2UeIU7ivtvf2DzsvJiHIqvQ2zLVGP_eyDrwskdtVYIsOF0k3NX5SeB77QijRMVhbaQkexRMHjwkXXkSKjakWTyjxYYMk0ZutU85wyhThDl5PXVc0Ruj3ZEdulqqbz94_WDkfhojEzklJSPsPJszyS-fjwSmS8BYFQ8S8X9KDPGlV5pWS14Hkw9Pi5ZbdKdgvVE_WWRz_G3cWd0gW-LlwWPTsmuGid7LCpOeHpLkrpCYoIIbvw'
            }
        })
            .should((response) => {
                expect(response.status).to.eq(200)
                expect(response).to.have.property('headers')
                expect(response).to.have.property('duration')
                expect(response.headers).to.have.property('content-type')
                expect(response.body).to.have.property('total', 8)
                expect(response.body).to.have.property('pages', 2)
                expect(response.body).to.have.property('currentPage', 1)
            })
    })
})
